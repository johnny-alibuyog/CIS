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
using ReactiveUI;

namespace CIS.UI.Features.Memberships.Users.MasterList
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : DialogBase, IViewFor<UserViewModel>
    {
        #region IViewFor<UserViewModel> Members

        public UserViewModel ViewModel
        {
            get { return this.DataContext as UserViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        } 

        #endregion

        public UserView()
        {
            this.InitializeComponent();
            //this.InitializeViewModelAsync(() => IoC.Container.Resolve<UserViewModel>());

            //if (!DesignerProperties.GetIsInDesignMode(this))
            //{
            //    this.ViewModel = new UserViewModel();
            //}
        }
    }
}
