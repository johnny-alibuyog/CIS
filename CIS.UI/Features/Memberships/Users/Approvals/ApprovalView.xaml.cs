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

namespace CIS.UI.Features.Memberships.Users.Approvals
{
    /// <summary>
    /// Interaction logic for ApprovalView.xaml
    /// </summary>
    public partial class ApprovalView : DialogBase, IViewFor<ApprovalViewModel>
    {
        public ApprovalView()
        {
            InitializeComponent();
        }

        #region IViewFor<ApprovalViewModel> Members

        public ApprovalViewModel ViewModel
        {
            get { return this.DataContext as ApprovalViewModel; }
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
