using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using CIS.Core.Domain.Common;
using CIS.Core.Domain.Membership;
using CIS.Data.Definition;
using CIS.Data.Definition.Common;
using CIS.Data.Definition.Membership;
using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate.Linq;

namespace CIS.UI.Features;

public class SplashScreenController : ControllerBase<SplashScreenViewModel>
{
    public SplashScreenController(SplashScreenViewModel viewModel)
        : base(viewModel)
    {
        var backgroundWorker = new BackgroundWorker();
        backgroundWorker.DoWork += (sender, e) => InitializeData();
        backgroundWorker.RunWorkerCompleted += (sender, e) => this.ViewModel.Close();
        backgroundWorker.RunWorkerAsync();
    }

    public virtual void InitializeData()
    {
        void Seed<Seeder>(string message) where Seeder : ISeeder
        {
            if (string.IsNullOrWhiteSpace(this.ViewModel.Licensee) && App.Context.Product != null)
                this.ViewModel.Licensee = App.Context.Product.Licensee;

            if (this.ViewModel.Plugins == null && App.Context.Product != null)
                this.ViewModel.Plugins = App.Context.Product.Plugins;

            this.ViewModel.Message = message;

            var seeder = IoC.Container.Resolve<Seeder>();

            if (seeder is ConfigurationSeeder.Product productSeeder)
            {
                productSeeder.Updated = (config) => App.Context.Product = config;
            }

            if (seeder is ConfigurationSeeder.ImageScaleFactor imageScaleFactorSeeder)
            {
                imageScaleFactorSeeder.Updated = (config) => App.Context.Image = config;
            }

            if (seeder is ConfigurationSeeder.DataStore dataStoreSeeder)
            {
                dataStoreSeeder.IsProductionEnvironment = () => App.Config.ConnectToProductionEnvironment;
                dataStoreSeeder.Updated = (config) => App.Context.DataStore = config;
            }

            if (seeder is TerminalSeeder terminalSeeder)
            {
                terminalSeeder.Updated = (config) => App.Context.Terminal = config;
            }

            seeder.Seed();

            Thread.Sleep(300);
        }

        Seed<ConfigurationSeeder.Product>("Initializing product configuration ...");

        Seed<ConfigurationSeeder.ProperCasing>("Initializing proper casing configuration ...");

        Seed<ConfigurationSeeder.DataStore>("Initializing data store configuration ...");

        Seed<ConfigurationSeeder.ImageScaleFactor>("Initializing image resize scale factore configuration ...");

        Seed<FingerSeeder>("Initializing finger print scanner configuration ...");

        Seed<TerminalSeeder>("Initializing terminal configuration ...");

        Seed<SettingSeeder>("Initializing police settings ...");

        Seed<RankSeeder>("Initializing police ranks ...");

        Seed<PurposeSeeder>("Initializing police purposes ...");

        Seed<StationSeeder>("Initializing police station ...");

        Seed<OfficerSeeder>("Initializing police officers ...");

        Seed<ApplicantionSeeder>("Initializing police clearance applicant ...");

        Seed<AddressSeeder>("Initializing address ...");

        //LoadImages();
    }

    private void LoadImages()
    {
        var applicants = default(List<Application>);

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            applicants = session.Query<Application>()
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
            foreach (var picture in applicant.Pictures.Where(x => x.Image != null))
            {
                var filename = Path.Combine(App.Config.ApplicationDataLocation, applicant.Person.Fullname + ".bmp");
                picture.Image.Save(filename);
            }
    }
}
