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

namespace CIS.UI.Features.Memberships.Users
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : DialogBase, IViewFor<LoginViewModel>
    {
        #region IViewFor<LoginViewModel> Members

        public LoginViewModel ViewModel
        {
            get { return this.DataContext as LoginViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public LoginView()
        {
            InitializeComponent();

            App.Config.Apprearance.Apply();
        }
    }
}
