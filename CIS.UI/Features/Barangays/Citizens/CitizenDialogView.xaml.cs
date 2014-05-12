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
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Citizens
{
    /// <summary>
    /// Interaction logic for CitizenDialogView.xaml
    /// </summary>
    public partial class CitizenDialogView : DialogBase, IViewFor<CitizenDialogViewModel>
    {
        #region IViewFor<CameraDialogViewModel> Members

        public CitizenDialogViewModel ViewModel
        {
            get { return this.DataContext as CitizenDialogViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public CitizenDialogView()
        {
            this.InitializeComponent();
            this.InitializeViewModelAsync(() => IoC.Container.Resolve<CitizenDialogViewModel>());
        }
    }
}
