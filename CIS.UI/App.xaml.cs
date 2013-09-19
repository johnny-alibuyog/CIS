using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Memberships;
using CIS.UI.Bootstraps.InversionOfControl;
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
            get
            {
                return Application.Current.Windows
                    .OfType<Window>()
                    .Where(x => x.IsActive)
                    .SingleOrDefault();
            }
        }

        public static ApplicationConfiguration Configuration { get; set; }

        public static class Data
        {
            public static User User { get; set; }
            public static City City { get; set; }
        }

        public App()
        {
            App.Configuration = IoC.Container.Resolve<ApplicationConfiguration>();
        }
    }
}
