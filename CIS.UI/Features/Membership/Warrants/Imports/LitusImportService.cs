using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.Core.Domain.Membership;
using CIS.Core.Utility.Extention;
using CIS.Data.Definition;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Membership.Warrants.Imports
{
    public class LitusImportService(ISessionFactory sessionFactory, ImportViewModel viewModel) : IImportService
    {
        private readonly ISessionFactory _sessionFactory = sessionFactory;
        private readonly ImportViewModel _viewModel = viewModel;

        private IEnumerable<LitusImportWarrant> ReadFromFile()
        {
            var directoryInfo = new DirectoryInfo(this._viewModel.SourcePath);
            var warrantFiles = directoryInfo.GetFiles("*arrest*xls*", SearchOption.AllDirectories).ToList();
            var suspectFiles = directoryInfo.GetFiles("*name*xls*", SearchOption.AllDirectories).ToList();

            var warrants = warrantFiles
                .Select(x => Importer.ImportExcel(x.FullName, LitusImportWarrant.OfDataRow))
                .SelectMany(x => x)
                .ToList();

            var suspects = suspectFiles
                .Select(x => Importer.ImportExcel(x.FullName, LitusImportSuspect.OfDataRow))
                .SelectMany(x => x)
                .ToList();

            var resultList = warrants
                .GroupJoin(suspects,
                    (warrant) => warrant.WarrantCode,
                    (suspect) => suspect.WarrantCode,
                    (warrant, joinedSuspects) => new LitusImportWarrant()
                    {
                        WarrantCode = warrant.WarrantCode,
                        CaseNumber = warrant.CaseNumber,
                        Description = warrant.Description,
                        BailAmount = warrant.BailAmount,
                        IssuedOn = warrant.IssuedOn,
                        IssuedBy = warrant.IssuedBy,
                        Address1 = warrant.Address1,
                        Address2 = warrant.Address2,
                        Barangay = warrant.Barangay,
                        City = warrant.City,
                        Province = warrant.Province,
                        Suspects = joinedSuspects
                    }
                )
                .Where(x =>
                    x.Suspects.All(o =>
                        !string.IsNullOrWhiteSpace(o.FirstName) &&
                            //string.IsNullOrWhiteSpace(o.MiddleName) ||
                        !string.IsNullOrWhiteSpace(o.LastName)
                    ) &&
                    x.Suspects.Count() > 0
                )
                .ToList();

            return resultList;
        }

        private void SaveBatch(int batchSize, IEnumerable<LitusImportWarrant> batch)
        {
            var batchCodesToImport = batch
                .Select(x => x.WarrantCode)
                .ToList();

            using var session = _sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();
            
            session.SetBatchSize(batchSize);

            var query = session.Query<Warrant>()
                .Where(x => batchCodesToImport.Contains(x.WarrantCode))
                .FetchMany(x => x.Suspects)
                .ThenFetchMany(x => x.Aliases)
                .ToFuture();

            session.Query<Warrant>()
                .Where(x => batchCodesToImport.Contains(x.WarrantCode))
                .FetchMany(x => x.Suspects)
                .ThenFetchMany(x => x.Occupations)
                .ToFuture();

            var warrantsInDb = query.ToList();

            foreach (var item in batch)
            {
                var warrant = warrantsInDb.FirstOrDefault(x => x.WarrantCode.IsEqualTo(item.WarrantCode)) ?? new Warrant();

                warrant.WarrantCode = item.WarrantCode;
                warrant.CaseNumber = item.CaseNumber;
                warrant.Crime = item.Description;
                warrant.BailAmount = item.BailAmount;
                warrant.IssuedOn = item.IssuedOn;
                warrant.IssuedBy = item.IssuedBy;
                warrant.IssuedAt = new Address()
                {
                    Address1 = item.Address1,
                    Address2 = item.Address2,
                    Barangay = item.Barangay,
                    City = item.City,
                    Province = item.Province,
                };

                foreach (var item1 in item.Suspects)
                {
                    var suspect = warrant.Suspects.FirstOrDefault(x =>
                        x.Person.FirstName.IsEqualTo(item1.FirstName) &&
                        x.Person.MiddleName.IsEqualTo(item1.MiddleName) &&
                        x.Person.LastName.IsEqualTo(item1.LastName) &&
                        x.Person.Suffix.IsEqualTo(item1.Suffix)
                    );

                    if (suspect == null)
                    {
                        suspect = new Suspect();
                        warrant.AddSuspect(suspect);
                    }
                    suspect.Aliases = new Collection<string>() { item1.AlsoKnownAs };
                    suspect.Person = new Person()
                    {
                        Prefix = item1.Prefix,
                        FirstName = item1.FirstName,
                        MiddleName = item1.MiddleName,
                        LastName = item1.LastName,
                        Suffix = item1.Suffix
                    };
                    suspect.Address = new Address()
                    {
                        Address1 = item.Address1,
                        Address2 = item.Address2,
                        Barangay = item.Barangay,
                        City = item.City,
                        Province = item.Province
                    };
                }

                session.SaveOrUpdate(warrant);

                this._viewModel.TotalCases++;
                this._viewModel.TotalSuspects += warrant.Suspects.Count();

                //if (warrant.Id == Guid.Empty)
                //    session.Insert(warrant);
                //else
                //    session.Update(warrant);

                //foreach (var item1 in warrant.Suspects)
                //{
                //    if (item1.Id == Guid.Empty)
                //        session.Insert(item1);
                //    else
                //        session.Update(item1);
                //}
            }

            transaction.Commit();
        }

        public void Execute()
        {
            ReadFromFile()
                .Chunk(size: 1000)
                .ForEach(batch => SaveBatch(
                    batchSize: batch.Count(), 
                    batch: batch
                ));
        }
    }
}
