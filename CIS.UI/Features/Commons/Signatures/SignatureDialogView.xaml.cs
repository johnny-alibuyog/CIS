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

namespace CIS.UI.Features.Commons.Signatures
{
    /// <summary>
    /// Interaction logic for SignatureViewDialog.xaml
    /// </summary>
    public partial class SignatureDialogView : DialogBase, IViewFor<SignatureDialogViewModel>
    {
        #region IViewFor<SignatureViewModel> Members

        public SignatureDialogViewModel ViewModel
        {
            get { return this.DataContext as SignatureDialogViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public SignatureDialogView()
        {
            InitializeComponent();

            this.ViewModel = new SignatureDialogViewModel();
        }
    }
}
