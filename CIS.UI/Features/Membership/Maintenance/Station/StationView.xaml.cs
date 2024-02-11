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
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Maintenance.Station
{
    /// <summary>
    /// Interaction logic for StationView.xaml
    /// </summary>
    public partial class StationView : UserControl, IViewFor<StationViewModel>
    {
        #region IViewFor<StationViewModel> Members

        public StationViewModel ViewModel
        {
            get { return this.DataContext as StationViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public StationView()
        {
            this.InitializeComponent();
            this.InitializeViewModel(() => IoC.Container.Resolve<StationViewModel>());
        }
    }
}
