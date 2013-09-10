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

namespace CIS.UI.Features.Firearms.Licenses
{
    /// <summary>
    /// Interaction logic for LicenseView.xaml
    /// </summary>
    public partial class LicenseView : DialogBase, IViewFor<LicenseViewModel>
    {
        public LicenseView()
        {
            InitializeComponent();
        }

        #region IViewFor<LicenseViewModel> Members

        public LicenseViewModel ViewModel
        {
            get { return this.DataContext as LicenseViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }

        } 
        #endregion
    }
}
