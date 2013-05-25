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
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Polices.Warrants
{
    public class LitusImportDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public virtual ImportViewModel ViewModel { get; set; }

        #region Routine Helpers

        private IEnumerable<ImportWarrantViewModel> ParseFromFile()
        {
            var directoryInfo = new DirectoryInfo(this.ViewModel.SourcePath);
            var warrantFiles = directoryInfo.GetFiles("*arrest*xls*", SearchOption.AllDirectories).ToList();
            var suspectFiles = directoryInfo.GetFiles("*name*xls*", SearchOption.AllDirectories).ToList();

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
            var warrants = new List<ImportWarrantViewModel>();
            foreach (var file in warrantFiles)
            {
                var excel = new ExcelQueryFactory(file.FullName);
                excel.AddMapping<ImportWarrantViewModel>(x => x.WarrantCode, "CODE");
                excel.AddMapping<ImportWarrantViewModel>(x => x.CaseNumber, "CASE_NO");
                excel.AddMapping<ImportWarrantViewModel>(x => x.Description, "CRIME_TYPE_NAME");
                excel.AddMapping<ImportWarrantViewModel>(x => x.BailAmount, "BAIL_AMOUNT");
                excel.AddMapping<ImportWarrantViewModel>(x => x.IssuedOn, "ISSUED_ON", x => ParseDate(x));
                excel.AddMapping<ImportWarrantViewModel>(x => x.IssuedBy, "ISSUED_BY_NAME");
                excel.AddMapping<ImportWarrantViewModel>(x => x.Address1, "ADDRESS1");
                excel.AddMapping<ImportWarrantViewModel>(x => x.Address2, "ADDRESS2");
                excel.AddMapping<ImportWarrantViewModel>(x => x.Barangay, "BARANGAY");
                excel.AddMapping<ImportWarrantViewModel>(x => x.City, "CITY");
                excel.AddMapping<ImportWarrantViewModel>(x => x.Province, "PROVINCE");

                var firstSheet = excel.GetWorksheetNames().First();
                warrants.AddRange(excel.Worksheet<ImportWarrantViewModel>(firstSheet).ToList());
            }

            // parse suspects
            var suspects = new List<ImportSuspectViewModel>();
            foreach (var file in suspectFiles)
            {
                var excel = new ExcelQueryFactory(file.FullName);
                excel.AddMapping<ImportSuspectViewModel>(x => x.WarrantCode, "WA_CODE");
                excel.AddMapping<ImportSuspectViewModel>(x => x.FirstName, "FNAME");
                excel.AddMapping<ImportSuspectViewModel>(x => x.MiddleName, "MNAME");
                excel.AddMapping<ImportSuspectViewModel>(x => x.LastName, "LNAME");
                excel.AddMapping<ImportSuspectViewModel>(x => x.Suffix, "SUF");
                excel.AddMapping<ImportSuspectViewModel>(x => x.AlsoKnownAs, "ALIAS");

                var firstSheet = excel.GetWorksheetNames().First();
                suspects.AddRange(excel.Worksheet<ImportSuspectViewModel>(firstSheet).ToList());
            }

            var resultList = warrants
                .GroupJoin(suspects,
                    (warrant) => warrant.WarrantCode,
                    (suspect) => suspect.WarrantCode,
                    (warrant, joinedSuspects) => new ImportWarrantViewModel()
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
                    x.Suspects
                        .All(o =>
                            !string.IsNullOrWhiteSpace(o.FirstName) &&
                                //string.IsNullOrWhiteSpace(o.MiddleName) ||
                            !string.IsNullOrWhiteSpace(o.LastName)
                        ) &&
                    x.Suspects.Count() > 0
                )
                .ToList();

            return resultList;
        }

        private void SaveBatch(int batchSize, IEnumerable<ImportWarrantViewModel> batchToImport)
        {
            var batchCodesToImport = batchToImport
                .Select(x => x.WarrantCode)
                .ToList();

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(batchSize);

                var warrantsInDb = session.Query<Warrant>()
                    .Where(x => batchCodesToImport.Contains(x.WarrantCode))
                    .FetchMany(x => x.Suspects)
                    .ThenFetchMany(x => x.Aliases)
                    .ToList();

                foreach (var item in batchToImport)
                {
                    var warrant = warrantsInDb.FirstOrDefault(x => string.Compare(x.WarrantCode, item.WarrantCode, true) == 0);
                    if (warrant == null)
                        warrant = new Warrant();

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
                        var suspect = warrant.Suspects
                            .Where(x =>
                                x.Person.FirstName == item1.FirstName &&
                                x.Person.MiddleName == item1.MiddleName &&
                                x.Person.LastName == item1.LastName &&
                                x.Person.Suffix == item1.Suffix)
                            .FirstOrDefault();

                        if (suspect == null)
                        {
                            suspect = new Suspect();
                            warrant.AddSuspect(suspect);
                        }
                        suspect.Aliases = new Collection<string>() { item1.AlsoKnownAs };
                        suspect.Person = new Person(
                            firstName: item1.FirstName,
                            middleName: item1.MiddleName,
                            lastName: item1.LastName,
                            suffix: item1.Suffix
                        );
                        suspect.Address = new Address(
                            address1: item.Address1,
                            address2: item.Address2,
                            barangay: item.Barangay,
                            city: item.City,
                            province: item.Province
                        );
                    }

                    session.SaveOrUpdate(warrant);

                    this.ViewModel.TotalCases++;
                    this.ViewModel.TotalSuspects += warrant.Suspects.Count();

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
        }

        #endregion

        #region Constructors

        public LitusImportDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        #endregion

        public void Execute()
        {
            var warrantsToImport = ParseFromFile();
            var warrantCodes = warrantsToImport.Select(x => x.WarrantCode).ToArray();

            var batchSize = 1100;
            var batchCount = warrantCodes.Count() / batchSize;
            var remainder = 0;
            Math.DivRem(batchCount, batchSize, out remainder);

            if (remainder != 0)
                batchCount++;

            if (batchCount == 0 && warrantCodes.Count() > 0)
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
