using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Memberships;

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

        public static class Data
        {
            public static User User { get; set; }
            public static City City { get; set; }
        }
    }
}
