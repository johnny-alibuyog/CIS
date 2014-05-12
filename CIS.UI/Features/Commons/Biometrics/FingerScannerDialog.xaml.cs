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

namespace CIS.UI.Features.Commons.Biometrics
{
    /// <summary>
    /// Interaction logic for FingerScannerDialog.xaml
    /// </summary>
    public partial class FingerScannerDialog : DialogBase, IViewFor<FingerScannerDialogViewModel>
    {
        #region IViewFor<FingerScannerDialogViewModel> Members

        public FingerScannerDialogViewModel ViewModel
        {
            get { return this.DataContext as FingerScannerDialogViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public FingerScannerDialog()
        {
            this.InitializeComponent();
            this.InitializeViewModelAsync(() => IoC.Container.Resolve<FingerScannerDialogViewModel>());

            this.Loaded += (sender, e) => this.ViewModel.FingerScanner.Start.Execute(null);
            this.Unloaded += (sender, e) => this.ViewModel.FingerScanner.Stop.Execute(null);
        }
    }
}
