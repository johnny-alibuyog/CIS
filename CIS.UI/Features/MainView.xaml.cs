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
using CIS.UI.Utilities.Extentions;
using FirstFloor.ModernUI.Windows.Controls;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;

namespace CIS.UI.Features
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ModernWindow, IViewFor<MainViewModel>
    {
        #region IViewFor<MainViewModel> Members

        public MainViewModel ViewModel
        {
            get { return this.DataContext as MainViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public MainView()
        {
            this.InitializeComponent();
            this.InitializeViewModelAsync(() => IoC.Container.Resolve<MainViewModel>());
        }
    }
}
