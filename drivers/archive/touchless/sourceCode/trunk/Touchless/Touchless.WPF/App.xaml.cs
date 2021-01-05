using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Touchless.Multitouch;
using Touchless.Vision.Service;

namespace Touchless.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static TouchlessInputProvider _touchlessInputProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ComposableTouchlessService touchlessService = ComposableTouchlessService.GetComposedInstance();
            _touchlessInputProvider = new TouchlessInputProvider(touchlessService);
            Application.Current.MainWindow = _touchlessInputProvider.GetConfiguration() as Window;
            Application.Current.MainWindow.Show();
            _touchlessInputProvider.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _touchlessInputProvider.Stop();
        }
    }
}
