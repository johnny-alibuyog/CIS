using System.Linq;
using System.Threading;
using System.Windows;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features;
using CIS.UI.Features.Security.Users.Logins;
using CIS.UI.Utilities.Configurations;
using CIS.UI.Utilities.Context;

namespace CIS.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Window CurrentWindow => Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

        public static ApplicationContext Context { get; set; }

        public static ApplicationConfiguration Config { get; set; }

        public App()
        {
            App.Context = IoC.Container.Resolve<ApplicationContext>();
            App.Config = IoC.Container.Resolve<ApplicationConfiguration>();
        }

        private Mutex _instanceMutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            _instanceMutex = new Mutex(true, @"Certification Issuance System", out var createdNew);
            if (createdNew == false)
            {
                _instanceMutex = null;
                MessageBox.Show("Application is already running...", "Certification Issuance System", MessageBoxButton.OK);
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);
            this.OnStartupExtracted();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            App.Config.Write();

            _instanceMutex?.ReleaseMutex();

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
    }
}
