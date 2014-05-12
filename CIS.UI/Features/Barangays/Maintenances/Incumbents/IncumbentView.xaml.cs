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
using System.Windows.Shapes;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Maintenances.Incumbents
{
    /// <summary>
    /// Interaction logic for IncumbentView.xaml
    /// </summary>
    public partial class IncumbentView : DialogBase, IViewFor<IncumbentViewModel>
    {
        #region IViewFor<IncumbentViewModel> Members

        public IncumbentViewModel ViewModel
        {
            get { return this.DataContext as IncumbentViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public IncumbentView()
        {
            InitializeComponent();
        }
    }
}
