using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate.Linq;

namespace CIS.UI.Features;

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
        dataInitializer = IoC.Container.Resolve<Commons.Configurations.ProductConfigurationDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing proper casing configuration ...");
        dataInitializer = IoC.Container.Resolve<Commons.Configurations.ProperCasingConfigurationDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing data store configuration ...");
        dataInitializer = IoC.Container.Resolve<Commons.Configurations.DataStoreConfigurationDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing image resize scale factore configuration ...");
        dataInitializer = IoC.Container.Resolve<Commons.Configurations.ImageScaleFactorConfigurationDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing finger print scanner configuration ...");
        dataInitializer = IoC.Container.Resolve<Commons.Biometrics.FingerDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing terminal configuration ...");
        dataInitializer = IoC.Container.Resolve<Commons.Terminals.TerminalDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing police settings ...");
        dataInitializer = IoC.Container.Resolve<Polices.Maintenances.Settings.SettingDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing police ranks ...");
        dataInitializer = IoC.Container.Resolve<Polices.Maintenances.Ranks.RankDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing police purposes ...");
        dataInitializer = IoC.Container.Resolve<Polices.Maintenances.Purposes.PurposeDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing police station ...");
        dataInitializer = IoC.Container.Resolve<Polices.Maintenances.Stations.StationDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing police clearance applicant ...");
        dataInitializer = IoC.Container.Resolve<Polices.Clearances.Archives.ApplicantDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing firearms make ...");
        dataInitializer = IoC.Container.Resolve<Firearms.Maintenances.Makes.MakeDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing firearms kind ...");
        dataInitializer = IoC.Container.Resolve<Firearms.Maintenances.Kinds.KindDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing barangay clearance purposes ...");
        dataInitializer = IoC.Container.Resolve<Barangays.Maintenances.Purposes.PurposeDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing barangay position ...");
        dataInitializer = IoC.Container.Resolve<Barangays.Maintenances.Positions.PositionDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing barangay settings ...");
        dataInitializer = IoC.Container.Resolve<Barangays.Maintenances.Settings.SettingDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Initializing barangay office ...");
        dataInitializer = IoC.Container.Resolve<Barangays.Maintenances.Offices.OfficeDataInitializer>();
        dataInitializer.Execute();

        SendMessageAndInitializeViewModel("Set proper casing configuration as initialzed...");
        var properCasingDataInitializer = (Commons.Configurations.ProperCasingConfigurationDataInitializer)null;
        properCasingDataInitializer = IoC.Container.Resolve<Commons.Configurations.ProperCasingConfigurationDataInitializer>();
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
