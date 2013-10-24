using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Memberships;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Commons.Biometrics;
using CIS.UI.Features.Commons.Terminals;
using CIS.UI.Features.Firearms.Maintenances;
using CIS.UI.Features.Polices.Maintenances;
using FirstFloor.ModernUI.Windows.Controls;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ModernWindow
    {
        public MainView()
        {
            InitializeComponent();

            App.Configuration.Apprearance.Apply();

            Task.Factory.StartNew(() =>
            {
                var dataInitializer = (IDataInitializer)null;

                //dataInitializer = IoC.Container.Resolve<AddressDataInitializer>();
                //dataInitializer.Execute();

                //var x = IoC.Container.Resolve<CIS.UI.Features.Polices.Clearances.ApplicationViewModel>();

                dataInitializer = IoC.Container.Resolve<FingerDataInitializer>();
                dataInitializer.Execute();

                dataInitializer = IoC.Container.Resolve<TerminalDataInitializer>();
                dataInitializer.Execute();

                dataInitializer = IoC.Container.Resolve<SettingDataInitializer>();
                dataInitializer.Execute();

                dataInitializer = IoC.Container.Resolve<RankDataInitializer>();
                dataInitializer.Execute();

                dataInitializer = IoC.Container.Resolve<PurposeDataInitializer>();
                dataInitializer.Execute();

                dataInitializer = IoC.Container.Resolve<MakeDataInitializer>();
                dataInitializer.Execute();

                dataInitializer = IoC.Container.Resolve<KindDataInitializer>();
                dataInitializer.Execute();

                dataInitializer = IoC.Container.Resolve<StationDataInitializer>();
                dataInitializer.Execute();
            }, TaskCreationOptions.LongRunning);
        }
    }
}
