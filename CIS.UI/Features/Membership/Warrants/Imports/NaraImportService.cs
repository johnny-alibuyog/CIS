using System.Collections.Generic;
using System.Data;
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
    /*
     use [cisdb];
     go

     select
         WarrantCode = isnull(w.WarrantCode, ''),
         CaseNumber= isnull(w.CaseNumber, ''),
         Crime = isnull(w.Crime, ''),
         Description = isnull(w.Description, ''),
         Remarks = isnull(w.Remarks, ''),
         BailAmount = isnull(w.BailAmount, 0),
         IssuedOn = isnull(w.IssuedOn, ''),
         IssuedBy = isnull(w.IssuedBy, ''),
         IssuedAtAddress1 = isnull(w.IssuedAtAddress1, ''),
         IssuedAtAddress2 = isnull(w.IssuedAtAddress2, ''),
         IssuedAtBarangay = isnull(w.IssuedAtBarangay, ''),
         IssuedAtCity = isnull(w.IssuedAtCity, ''),
         IssuedAtProvince = isnull(w.IssuedAtProvince, ''),
         FirstName = isnull(s.FirstName, ''),
         MiddleName = isnull(s.MiddleName, ''),
         LastName = isnull(s.LastName, ''),
         Suffix = isnull(s.Suffix, ''),
         Gender = isnull(s.Gender, ''),
         BirthDate = isnull(s.BirthDate, ''),
         Address1 = isnull(s.Address1, ''),
         Address2 = isnull(s.Address2, ''),
         Barangay = isnull(s.Barangay, ''),
         City = isnull(s.City, ''),
         Province = isnull(s.Province, ''),
         Hair = isnull(s.Hair, ''),
         Eyes = isnull(s.Eyes, ''),
         Complexion = isnull(s.Complexion, ''),
         Build = isnull(s.Build, ''),
         ScarsAndMarks = isnull(s.ScarsAndMarks, ''),
         Race = isnull(s.Race, ''),
         Nationality = isnull(s.Nationality, '')
     from 
         Membership.Warrants as w
     inner join
         Membership.Suspects as s
             on w.WarrantId = s.WarrantId
     where
         w.CaseNumber <> '' and
         w.CaseNumber is not null     
    */

    public class NaraImportService(ISessionFactory sessionFactory, ImportViewModel viewModel) : IImportService
    {
        private readonly ISessionFactory _sessionFactory = sessionFactory;
        private readonly ImportViewModel _viewModel = viewModel;

        private IEnumerable<NaraImportWarrant> ReadFromFile()
        {
            var directoryInfo = new DirectoryInfo(this._viewModel.SourcePath);
            var warrantFiles = directoryInfo.GetFiles("*warrant_list*xls*", SearchOption.AllDirectories).ToList();

            var mapper = (NaraImportWarrant Warrant, NaraImportSuspect Suspect) (DataRow row) =>
                (NaraImportWarrant.OfDataRow(row), NaraImportSuspect.OfDataRow(row));

            var raw = warrantFiles
                .Select(x => Importer.ImportExcel(x.FullName, mapper))
                .SelectMany(x => x)
                .ToList();

            var warrants = raw.Select(item => item.Warrant).ToList();
            var suspects = raw.Select(item => item.Suspect).Where(NaraImportSuspect.IsValid).ToList();

            // ensure uniqueness
            warrants = warrants
                .GroupBy(x => new
                {
                    x.CaseNumber,
                    x.Crime
                })
                .Select(x => x.FirstOrDefault())
                .ToList();

            var resultList = warrants
                .GroupJoin(suspects,
                    (warrant) => new
                    {
                        warrant.CaseNumber,
                        warrant.Crime
                    },
                    (suspect) => new
                    {
                        suspect.CaseNumber,
                        suspect.Crime
                    },
                    (warrant, joinedSuspects) => new NaraImportWarrant()
                    {
                        WarrantCode = warrant.WarrantCode,
                        CaseNumber = warrant.CaseNumber,
                        Crime = warrant.Crime,
                        Remarks = warrant.Remarks,
                        BailAmount = warrant.BailAmount,
                        IssuedOn = warrant.IssuedOn,
                        IssuedBy = warrant.IssuedBy,
                        IssuedAtAddress1 = warrant.IssuedAtAddress1,
                        IssuedAtAddress2 = warrant.IssuedAtAddress2,
                        IssuedAtBarangay = warrant.IssuedAtBarangay,
                        IssuedAtCity = warrant.IssuedAtCity,
                        IssuedAtProvince = warrant.IssuedAtProvince,
                        Suspects = joinedSuspects
                    }
                )
                .Where(x => x.Suspects.Count() > 0)
                .ToList();

            return resultList;
        }

        private void SaveBatch(int batchSize, IEnumerable<NaraImportWarrant> batch)
        {
            var batchCodesToImport = batch
                .Select(x => x.CaseNumber)
                .ToList();

            using var session = _sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();
            
            session.SetBatchSize(batchSize);

            var query = session.Query<Warrant>()
                .Where(x => batchCodesToImport.Contains(x.CaseNumber))
                .FetchMany(x => x.Suspects)
                .ThenFetchMany(x => x.Aliases)
                .ToFuture();

            session.Query<Warrant>()
                .Where(x => batchCodesToImport.Contains(x.CaseNumber))
                .FetchMany(x => x.Suspects)
                .ThenFetchMany(x => x.Occupations)
                .ToFuture();

            var warrantsInDb = query.ToList();

            foreach (var item in batch)
            {
                var warrant = warrantsInDb.FirstOrDefault(x => x.CaseNumber.IsEqualTo(item.CaseNumber)) ?? new Warrant();

                warrant.WarrantCode = item.WarrantCode;
                warrant.CaseNumber = item.CaseNumber;
                warrant.Crime = item.Crime;
                warrant.Remarks = item.Remarks;
                warrant.BailAmount = item.BailAmount;
                warrant.IssuedOn = item.IssuedOn;
                warrant.IssuedBy = item.IssuedBy;
                warrant.IssuedAt = new Address()
                {
                    Address1 = item.IssuedAtAddress1,
                    Address2 = item.IssuedAtAddress2,
                    Barangay = item.IssuedAtBarangay,
                    City = item.IssuedAtCity,
                    Province = item.IssuedAtProvince,
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
                    //suspect.Aliases = new Collection<string>() { item1.AlsoKnownAs };
                    suspect.Person = new Person()
                    {
                        Prefix = item1.Prefix,
                        FirstName = item1.FirstName,
                        MiddleName = item1.MiddleName,
                        LastName = item1.LastName,
                        Suffix = item1.Suffix,
                        Gender = item1.Gender.AsNullableEnum<Gender>(),
                        BirthDate = item1.BirthDate
                    };
                    suspect.Address = new Address()
                    {
                        Address1 = item1.Address1,
                        Address2 = item1.Address2,
                        Barangay = item1.Barangay,
                        City = item1.City,
                        Province = item1.Province
                    };
                    suspect.PhysicalAttributes = new PhysicalAttribute()
                    {
                        Hair = item1.Hair,
                        Eyes = item1.Eyes,
                        Complexion = item1.Complexion,
                        Build = item1.Build,
                        ScarsAndMarks = item1.ScarsAndMarks,
                        Race = item1.Race,
                        Nationality = item1.Nationality,
                    };
                }

                session.SaveOrUpdate(warrant);

                this._viewModel.TotalCases++;
                this._viewModel.TotalSuspects += warrant.Suspects.Count();
            }

            transaction.Commit();
        }

        public void Execute()
        {
            ReadFromFile()
                .Chunk(size: 1100)
                .ForEach(batch => SaveBatch(
                    batchSize: batch.Count(), 
                    batch: batch
                ));
        }
    }
}
