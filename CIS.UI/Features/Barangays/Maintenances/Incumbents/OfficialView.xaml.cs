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
    /// Interaction logic for OfficialView.xaml
    /// </summary>
    public partial class OfficialView : DialogBase, IViewFor<OfficialViewModel>
    {
        #region IViewFor<OfficialViewModel> Members

        public OfficialViewModel ViewModel
        {
            get { return this.DataContext as OfficialViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public OfficialView()
        {
            InitializeComponent();
        }
    }
}
