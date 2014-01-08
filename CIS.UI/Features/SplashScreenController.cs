using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Commons.Biometrics;
using CIS.UI.Features.Commons.Configurations;
using CIS.UI.Features.Commons.Terminals;
using CIS.UI.Features.Firearms.Maintenances;
using CIS.UI.Features.Polices.Maintenances;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features
{
    public class SplashScreenController : ControllerBase<SplashScreenViewModel>
    {
        private readonly BackgroundWorker _backgroundWorker;

        public SplashScreenController(SplashScreenViewModel viewModel)
            : base(viewModel)
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += (sender, e) => InitializeData();
            _backgroundWorker.RunWorkerCompleted += (sender, e) => this.ViewModel.Close();
            _backgroundWorker.RunWorkerAsync();
        }

        public virtual void InitializeData()
        {
            var dataInitializer = (IDataInitializer)null;

            Action<string> SendMessageAndInitializeViewModel = (message) =>
            {
                this.ViewModel.Message = message;
                Thread.Sleep(300);

                if (string.IsNullOrWhiteSpace(this.ViewModel.Licensee) && App.Data.Product != null)
                    this.ViewModel.Licensee = App.Data.Product.Licensee;

                if (this.ViewModel.Plugins == null && App.Data.Product != null)
                    this.ViewModel.Plugins = App.Data.Product.Plugins;
            };

            //dataInitializer = IoC.Container.Resolve<AddressDataInitializer>();
            //dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing product configuration ...");
            dataInitializer = IoC.Container.Resolve<ProductConfigurationDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing proper casing configuration ...");
            dataInitializer = IoC.Container.Resolve<ProperCasingConfigurationDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing data store configuration ...");
            dataInitializer = IoC.Container.Resolve<DataStoreConfigurationDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing image resize scale factore configuration ...");
            dataInitializer = IoC.Container.Resolve<ImageScaleFactorConfigurationDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing finger print scanner configuration ...");
            dataInitializer = IoC.Container.Resolve<FingerDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing terminal configuration ...");
            dataInitializer = IoC.Container.Resolve<TerminalDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing settings ...");
            dataInitializer = IoC.Container.Resolve<SettingDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing ranks ...");
            dataInitializer = IoC.Container.Resolve<RankDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing purposes ...");
            dataInitializer = IoC.Container.Resolve<PurposeDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing make ...");
            dataInitializer = IoC.Container.Resolve<MakeDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing kind ...");
            dataInitializer = IoC.Container.Resolve<KindDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Initializing station ...");
            dataInitializer = IoC.Container.Resolve<StationDataInitializer>();
            dataInitializer.Execute();

            SendMessageAndInitializeViewModel("Set proper casing configuration as initialzed...");
            var properCasingDataInitializer = (ProperCasingConfigurationDataInitializer)null;
            properCasingDataInitializer = IoC.Container.Resolve<ProperCasingConfigurationDataInitializer>();
            properCasingDataInitializer.SetProperCasingIsInitialized(true);

            //LoadImages();
        }

        private void LoadImages()
        {
            var applicants = new List<Applicant>();

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                applicants = session.Query<Applicant>()
                    .Where(x =>
                        x.Person.Gender == Gender.Female &&
                        x.CivilStatus == CivilStatus.Single &&
                        DateTime.Today.Year - x.Person.BirthDate.Value.Year < 21
                    )
                    .Fetch(x => x.Pictures)
                    .ToList();

                transaction.Commit();
            }

            foreach (var applicant in applicants)
                foreach (var picture in applicant.Pictures)
                {
                    if (picture.Image == null)
                        continue;

                    var filename = Path.Combine(App.Config.ApplicationDataLocation, applicant.Person.Fullname + ".bmp");
                    picture.Image.Save(filename);
                }
        }
    }
}
