using System;
using System.Collections.Generic;
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

namespace CIS.UI.Features.Polices.Warrants
{
    public class CodelessImportService : IImportService
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ImportViewModel _viewModel;

        #region Routine Helpers

        private IEnumerable<CodelessWarrant> ParseFromFile()
        {
            var directoryInfo = new DirectoryInfo(this._viewModel.SourcePath);
            var warrantFiles = directoryInfo.GetFiles("*codeless*xls*", SearchOption.AllDirectories).ToList();

            // parse warrants
            var warrants = new List<CodelessWarrant>();
            foreach (var file in warrantFiles)
            {
                var excel = new ExcelQueryFactory(file.FullName);
                excel.AddMapping<CodelessWarrant>(x => x.LastName, "SURNAME");
                excel.AddMapping<CodelessWarrant>(x => x.MiddleName, "GIVEN NAME");
                excel.AddMapping<CodelessWarrant>(x => x.FirstName, "MIDDLE NAME");
                //excel.AddMapping<CodelessWarrant>(x => x.Suffix, "BAIL_AMOUNT");
                excel.AddMapping<CodelessWarrant>(x => x.Address, "ADDRESS");
                excel.AddMapping<CodelessWarrant>(x => x.Case, "CASE");
                excel.AddMapping<CodelessWarrant>(x => x.Disposition, "DISPOSITION");
                excel.AddMapping<CodelessWarrant>(x => x.DateArrested, "DATE ARRESTED");

                var firstSheet = excel.GetWorksheetNames().First();
                warrants.AddRange(excel.Worksheet<CodelessWarrant>(firstSheet).ToList());
            }

            return warrants;
        }

        private void SaveBatch(int batchSize, IEnumerable<CodelessWarrant> batchToImport)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(batchSize);

                var warrants = new List<Warrant>();

                foreach (var item in batchToImport)
                {
                    var exists = session.Query<Suspect>().Any(x =>
                        x.Person.FirstName == item.FirstName &&
                        x.Person.MiddleName == item.MiddleName &&
                        x.Person.LastName == item.LastName &&
                        x.Warrant.Crime == item.Case
                    );

                    if (exists)
                        continue;

                    warrants.Add(new Warrant()
                    {
                        Crime = item.Case,
                        Description = item.DateArrested != null ? string.Format("Arrested on {0}.", item.DateArrested.Value.ToString("MMM dd, yyyy")) : null,
                        Remarks = item.Disposition,
                        Suspects = new List<Suspect>()
                        {
                            new Suspect()
                            {
                                Person = new Person()
                                {
                                    LastName = item.LastName,
                                    MiddleName = item.MiddleName,
                                    FirstName = item.FirstName
                                },
                                Address = new Address()
                                {
                                    City = item.Address
                                }
                            }
                        },

                    });
                }

                foreach (var item in warrants)
                {
                    session.Save(item);

                    this._viewModel.TotalCases++;
                    this._viewModel.TotalSuspects++;
                }

                transaction.Commit();
            }
        }

        #endregion

        public CodelessImportService(ISessionFactory sessionFactory, ImportViewModel viewModel)
        {
            _sessionFactory = sessionFactory;
            _viewModel = viewModel;
        }

        public void Execute()
        {
            var parsed = ParseFromFile();
            var warrantsToImport = parsed.Where(x =>
                string.IsNullOrWhiteSpace(x.LastName) == false &&
                string.IsNullOrWhiteSpace(x.FirstName) == false
            )
            .ToList();

            var batchSize = 1100;
            var batchCount = warrantsToImport.Count() / batchSize;
            var remainder = 0;
            Math.DivRem(batchCount, batchSize, out remainder);

            if (remainder != 0)
                batchCount++;

            if (batchCount == 0 && warrantsToImport.Count() > 0)
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
