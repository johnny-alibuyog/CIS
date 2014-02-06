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

namespace CIS.UI.Features.Polices.Maintenances.Officers
{
    /// <summary>
    /// Interaction logic for OfficerView.xaml
    /// </summary>
    public partial class OfficerView : DialogBase, IViewFor<OfficerViewModel>
    {
        #region IViewFor<OfficerViewModel> Members

        public OfficerViewModel ViewModel
        {
            get { return this.DataContext as OfficerViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public OfficerView()
        {
            InitializeComponent();
        }
    }
}
