using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Firearms;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Firearms.Maintenances;
using CIS.UI.Utilities.Extentions;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class ImportService : IImportService
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ImportViewModel _viewModel;

        #region  Routine Helpers

        private IEnumerable<ImportLicenseViewModel> ParseFromFile()
        {
            var directoryInfo = new DirectoryInfo(this._viewModel.SourcePath);
            var licenseFiles = directoryInfo.GetFiles("*license*xls*", SearchOption.AllDirectories).ToList();

            var licenses = new List<ImportLicenseViewModel>();
            foreach (var file in licenseFiles)
            {
                var excel = new ExcelQueryFactory(file.FullName);
                var firstSheet = excel.GetWorksheetNames().First();
                licenses.AddRange(excel.Worksheet<ImportLicenseViewModel>(firstSheet).ToList());
            }

            // ensure uniqueness by LicenseNumber
            return licenses
                .GroupBy(x => x.LicenseNumber)
                .Select(x => x.First())
                .ToList();
        }

        private void SaveBatch(int batchSize, IEnumerable<ImportLicenseViewModel> batchToImport)
        {
            var batchLicensNumbersToImport = batchToImport
                .Select(x => x.LicenseNumber)
                .ToList();

            using (var session = _sessionFactory.OpenStatelessSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(batchSize);

                var licensesInDb = session.Query<License>()
                    .Where(x => batchLicensNumbersToImport.Contains(x.LicenseNumber))
                    .ToList();

                var kinds = session.Query<Kind>().Cacheable().ToList();
                var makes = session.Query<Make>().Cacheable().ToList();

                foreach (var item in batchToImport)
                {
                    var license = licensesInDb.FirstOrDefault(x => string.Compare(x.LicenseNumber, item.LicenseNumber, true) == 0);
                    if (license == null)
                    {
                        license = new License();
                        license.LicenseNumber = item.LicenseNumber;
                        license.ControlNumber = item.ControlNumber;
                        license.IssueDate = item.IssueDate;
                        license.ExpiryDate = item.ExpiryDate;
                        license.Person = new Person()
                        {
                            FirstName = item.FirstName,
                            MiddleName = item.MiddleName,
                            LastName = item.LastName,
                            Suffix = item.Suffix,
                            Gender = item.Gender.As<Gender>(),
                            BirthDate = item.BirthDate
                        };
                        license.Address = new Address()
                        {
                            Address1 = item.Address1,
                            Address2 = item.Address2,
                            Barangay = item.Barangay,
                            City = item.City,
                            Province = item.Province,
                        };
                        license.Gun = new Gun()
                        {
                            Model = item.Model,
                            Caliber = item.Caliber,
                            SerialNumber = item.SerialNumber,
                            Kind = kinds.FirstOrDefault(x => string.Compare(x.Name, item.Kind, true) == 0),
                            Make = makes.FirstOrDefault(x => string.Compare(x.Name, item.Make, true) == 0)
                        };

                        session.Insert(license);

                        this._viewModel.TotalLicenses++;
                    }
                }

                transaction.Commit();
            }
        }

        #endregion

        #region Constructors

        public ImportService(ISessionFactory sessionFactory, ImportViewModel viewModel)
        {
            _sessionFactory = sessionFactory;
            _viewModel = viewModel;
        }

        #endregion

        public void Execute()
        {
            var licensesToImport = ParseFromFile();
            var licenseNumber = licensesToImport.Select(x => x.LicenseNumber).ToArray();

            var batchSize = 1100;
            var batchCount = licenseNumber.Count() / batchSize;
            var remainder = 0;
            Math.DivRem(batchCount, batchSize, out remainder);

            if (remainder != 0)
                batchCount++;

            if (batchCount == 0 && licenseNumber.Count() > 0)
                batchCount = 1;

            // get new lookup data(Kind, Make)
            var kindDataInitializer = IoC.Container.Resolve<KindDataInitializer>();
            kindDataInitializer.Data = licensesToImport.Select(x => x.Kind).Distinct();
            kindDataInitializer.Execute();

            var makeDataInitializer = IoC.Container.Resolve<MakeDataInitializer>();
            makeDataInitializer.Data = licensesToImport.Select(x => x.Make).Distinct();
            makeDataInitializer.Execute();

            for (var batchNumber = 0; batchNumber < batchCount; batchNumber++)
            {
                var batchToImport = licensesToImport
                    .Skip(batchSize * batchNumber)
                    .Take(batchSize);

                SaveBatch(batchSize, batchToImport);
            }
        }
    }
}
