using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using FirstFloor.ModernUI.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Warrants.MasterList
{
    /// <summary>
    /// Interaction logic for WarratnView.xaml
    /// </summary>
    public partial class WarrantView : DialogBase, IViewFor<WarrantViewModel>
    {
        #region IViewFor<WarrantViewModel> Members

        public WarrantViewModel ViewModel
        {
            get { return this.DataContext as WarrantViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public WarrantView() : base()
        {
            this.InitializeComponent();

            //if (!DesignerProperties.GetIsInDesignMode(this))
            //{
            //    this.ViewModel = new WarrantViewModel();
            //}
        }
    }
}
