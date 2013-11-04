using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.Core.Utilities.Extentions;
using CIS.UI.Utilities.Extentions;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Polices.Warrants
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
         Polices.Warrants as w
     inner join
         Polices.Suspects as s
             on w.WarrantId = s.WarrantId
     where
         w.CaseNumber <> '' and
         w.CaseNumber is not null     
    */

    public class NaraImportService : IImportService
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ImportViewModel _viewModel;

        #region Constructors

        public NaraImportService(ISessionFactory sessionFactory, ImportViewModel viewModel)
        {
            _sessionFactory = sessionFactory;
            _viewModel = viewModel;
        }

        #endregion

        #region Routine Helpers

        private IEnumerable<NaraImportWarrant> ParseFromFile()
        {
            var directoryInfo = new DirectoryInfo(this._viewModel.SourcePath);
            var warrantFiles = directoryInfo.GetFiles("*warrant_list*xls*", SearchOption.AllDirectories).ToList();
            //var suspectFiles = directoryInfo.GetFiles("*name*xls*", SearchOption.AllDirectories).ToList();

            Func<string, DateTime> ParseDate = (value) =>
            {
                DateTime result;
                DateTime.TryParse(value, out result);

                if (result > SqlDateTime.MinValue.Value)
                    return result;
                else
                    return SqlDateTime.MinValue.Value;
            };

            // parse warrants
            var warrants = new List<NaraImportWarrant>();
            var suspects = new List<NaraImportSuspect>();

            foreach (var file in warrantFiles)
            {
                var excel = new ExcelQueryFactory(file.FullName);
                var firstSheet = excel.GetWorksheetNames().First();
                var tempWarrants = excel.Worksheet<NaraImportWarrant>(firstSheet).ToList();
                var tempSuspects = excel.Worksheet<NaraImportSuspect>(firstSheet).ToList();

                warrants.AddRange(tempWarrants);
                suspects.AddRange(tempSuspects);
            }

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
                        CaseNumber = warrant.CaseNumber.ToProperCase(),
                        Crime = warrant.Crime.ToProperCase()
                    },
                    (suspect) => new
                    {
                        CaseNumber = suspect.CaseNumber.ToProperCase(),
                        Crime = suspect.Crime.ToProperCase()
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

        private void SaveBatch(int batchSize, IEnumerable<NaraImportWarrant> batchToImport)
        {
            var batchCodesToImport = batchToImport
                .Select(x => x.CaseNumber)
                .ToList();

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(batchSize);

                var warrantsInDb = session.Query<Warrant>()
                    .Where(x => batchCodesToImport.Contains(x.CaseNumber))
                    .FetchMany(x => x.Suspects)
                    .ThenFetchMany(x => x.Aliases)
                    .ToList();

                foreach (var item in batchToImport)
                {
                    var warrant = warrantsInDb.FirstOrDefault(x => x.CaseNumber.IsEqualTo(item.CaseNumber));
                    if (warrant == null)
                        warrant = new Warrant();

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
                            FirstName = item1.FirstName,
                            MiddleName = item1.MiddleName,
                            LastName = item1.LastName,
                            Suffix = item1.Suffix,
                            Gender = item1.Gender.As<Gender>(),
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
                        suspect.PhysicalAttributes = new PhysicalAttributes()
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
        }

        #endregion

        public void Execute()
        {
            var warrantsToImport = ParseFromFile();
            var caseNumber = warrantsToImport.Select(x => x.CaseNumber).ToArray();

            var batchSize = 1100;
            var batchCount = caseNumber.Count() / batchSize;
            var remainder = 0;
            Math.DivRem(batchCount, batchSize, out remainder);

            if (remainder != 0)
                batchCount++;

            if (batchCount == 0 && caseNumber.Count() > 0)
                batchCount = 1;

            for (var batchNumber = 0; batchNumber < batchCount; batchNumber++)
            {
                var batchToImport = warrantsToImport
                    .Skip(batchSize * batchNumber)
                    .Take(batchSize);

                SaveBatch(batchSize, batchToImport);
            }

        }
    }
}
