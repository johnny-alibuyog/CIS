using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Memberships;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features;
using CIS.UI.Features.Memberships.Users;
using CIS.UI.Utilities;
using CIS.UI.Utilities.Configurations;

namespace CIS.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Window CurrentWindow
        {
            get { return Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive); }
        }

        public static ApplicationData Data { get; set; }

        public static ApplicationConfiguration Config { get; set; }

        public App()
        {
            App.Data = IoC.Container.Resolve<ApplicationData>();
            App.Config = IoC.Container.Resolve<ApplicationConfiguration>();
        }

        private Mutex _instanceMutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            var createdNew = false;
            _instanceMutex = new Mutex(true, @"Clearance Issuance System", out createdNew);
            if (createdNew == false)
            {
                _instanceMutex = null;
                MessageBox.Show("Application is already running...", "Clearance Issuance System", MessageBoxButton.OK);
                Application.Current.Shutdown();
                return;
            }

            base.OnStartup(e);
            this.OnStartupExtracted();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //App.Config.Provider.Write(App.Config);
            App.Config.Write();

            if (_instanceMutex != null)
                _instanceMutex.ReleaseMutex();

            base.OnExit(e);
        }

        private void OnStartupExtracted()
        {
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // authentication (login / registration)
            var loginDialog = new DialogService<LoginViewModel>();
            loginDialog.ShowModal();
            var actionResult = loginDialog.ViewModel.ActionResult;
            if (actionResult == null || actionResult.Value == false)
            {
                this.Shutdown(1);
                return;
            }

            // splash screen
            var splashDialog = new DialogService<SplashScreenViewModel>();
            splashDialog.ShowModal();

            // main screen
            var mainDialog = new DialogService<MainViewModel>();
            this.MainWindow = mainDialog.View;
            this.MainWindow.ShowDialog();
            this.Shutdown(1);
        }

        public class ApplicationData
        {
            public virtual User User { get; set; }
            public virtual City City { get; set; }
            public virtual ProductConfiguration Product { get; set; }
            public virtual DataStoreConfiguration DataStore { get; set; }
            public virtual ImageScaleFactorConfiguration Image { get; set; }
        }
    }
}
