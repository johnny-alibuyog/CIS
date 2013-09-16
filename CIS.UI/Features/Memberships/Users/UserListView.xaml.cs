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
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Memberships.Users
{
    /// <summary>
    /// Interaction logic for UserListView.xaml
    /// </summary>
    public partial class UserListView : UserControl, IViewFor<UserListViewModel>
    {
        public UserListView()
        {
            InitializeComponent();

            this.CreateViewModel(() => IoC.Container.Resolve<UserListViewModel>());

            //this.ViewModel = new UserListViewModel();
        }

        #region IViewFor<UserListViewModel>

        public UserListViewModel ViewModel
        {
            get { return this.DataContext as UserListViewModel; }
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
