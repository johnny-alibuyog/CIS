﻿using System;
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
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Polices.Warrants.Imports
{
    public class LitusImportService : IImportService
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ImportViewModel _viewModel;

        #region Routine Helpers

        private IEnumerable<LitusImportWarrant> ParseFromFile()
        {
            var directoryInfo = new DirectoryInfo(this._viewModel.SourcePath);
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
            var warrants = new List<LitusImportWarrant>();
            foreach (var file in warrantFiles)
            {
                var excel = new ExcelQueryFactory(file.FullName);
                excel.AddMapping<LitusImportWarrant>(x => x.WarrantCode, "CODE");
                excel.AddMapping<LitusImportWarrant>(x => x.CaseNumber, "CASE_NO");
                excel.AddMapping<LitusImportWarrant>(x => x.Description, "CRIME_TYPE_NAME");
                excel.AddMapping<LitusImportWarrant>(x => x.BailAmount, "BAIL_AMOUNT");
                excel.AddMapping<LitusImportWarrant>(x => x.IssuedOn, "ISSUED_ON", x => ParseDate(x));
                excel.AddMapping<LitusImportWarrant>(x => x.IssuedBy, "ISSUED_BY_NAME");
                excel.AddMapping<LitusImportWarrant>(x => x.Address1, "ADDRESS1");
                excel.AddMapping<LitusImportWarrant>(x => x.Address2, "ADDRESS2");
                excel.AddMapping<LitusImportWarrant>(x => x.Barangay, "BARANGAY");
                excel.AddMapping<LitusImportWarrant>(x => x.City, "CITY");
                excel.AddMapping<LitusImportWarrant>(x => x.Province, "PROVINCE");

                var firstSheet = excel.GetWorksheetNames().First();
                warrants.AddRange(excel.Worksheet<LitusImportWarrant>(firstSheet).ToList());
            }

            // parse suspects
            var suspects = new List<LitusImportSuspect>();
            foreach (var file in suspectFiles)
            {
                var excel = new ExcelQueryFactory(file.FullName);
                excel.AddMapping<LitusImportSuspect>(x => x.WarrantCode, "WA_CODE");
                excel.AddMapping<LitusImportSuspect>(x => x.FirstName, "FNAME");
                excel.AddMapping<LitusImportSuspect>(x => x.MiddleName, "MNAME");
                excel.AddMapping<LitusImportSuspect>(x => x.LastName, "LNAME");
                excel.AddMapping<LitusImportSuspect>(x => x.Suffix, "SUF");
                excel.AddMapping<LitusImportSuspect>(x => x.AlsoKnownAs, "ALIAS");

                var firstSheet = excel.GetWorksheetNames().First();
                suspects.AddRange(excel.Worksheet<LitusImportSuspect>(firstSheet).ToList());
            }

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

        private void SaveBatch(int batchSize, IEnumerable<LitusImportWarrant> batchToImport)
        {
            var batchCodesToImport = batchToImport
                .Select(x => x.WarrantCode)
                .ToList();

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
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

                foreach (var item in batchToImport)
                {
                    var warrant = warrantsInDb.FirstOrDefault(x => x.WarrantCode.IsEqualTo(item.WarrantCode));
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
        }

        #endregion

        #region Constructors

        public LitusImportService(ISessionFactory sessionFactory, ImportViewModel viewModel)
        {
            _sessionFactory = sessionFactory;
            _viewModel = viewModel;
        }

        #endregion

        public void Execute()
        {
            var warrantsToImport = ParseFromFile();
            var warrantCodes = warrantsToImport.Select(x => x.WarrantCode).ToArray();

            var batchSize = 1000;
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
