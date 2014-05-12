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

namespace CIS.UI.Features.Commons.Cameras
{
    /// <summary>
    /// Interaction logic for CameraDialogView.xaml
    /// </summary>
    public partial class CameraDialogView : DialogBase, IViewFor<CameraDialogViewModel>
    {
        #region IViewFor<CameraDialogViewModel> Members

        public CameraDialogViewModel ViewModel
        {
            get { return this.DataContext as CameraDialogViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public CameraDialogView()
        {
            this.InitializeComponent();
            this.InitializeViewModelAsync(() => IoC.Container.Resolve<CameraDialogViewModel>());

            this.Loaded += (sender, e) => this.ViewModel.Camera.Start.Execute(null);
            this.Unloaded += (sender, e) => this.ViewModel.Camera.Stop.Execute(null);
        }
    }
}
