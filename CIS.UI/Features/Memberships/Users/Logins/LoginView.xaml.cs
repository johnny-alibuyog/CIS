using ReactiveUI;

namespace CIS.UI.Features.Memberships.Users.Logins
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
