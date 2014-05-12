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

namespace CIS.UI.Features.Barangays.Citizens
{
    /// <summary>
    /// Interaction logic for CitizenView.xaml
    /// </summary>
    public partial class CitizenView : UserControl, IViewFor<CitizenViewModel>
    {
        #region IViewFor<CitizenViewModel> Members

        public CitizenViewModel ViewModel
        {
            get { return this.DataContext as CitizenViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public CitizenView()
        {
            InitializeComponent();
        }
    }
}
