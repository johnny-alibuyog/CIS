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
using ReactiveUI;
using CIS.UI.Utilities.Extentions;
using CIS.UI.Bootstraps.InversionOfControl;

namespace CIS.UI.Features.Barangays.Maintenances.Incumbents
{
    /// <summary>
    /// Interaction logic for IncumbentListView.xaml
    /// </summary>
    public partial class IncumbentListView : UserControl, IViewFor<IncumbentListViewModel>
    {
        #region IViewFor<IncumbentListViewModel> Members

        public IncumbentListViewModel ViewModel
        {
            get { return this.DataContext as IncumbentListViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion
        
        public IncumbentListView()
        {
            this.InitializeComponent();
            this.InitializeViewModelAsync(() => IoC.Container.Resolve<IncumbentListViewModel>());
        }
    }
}
