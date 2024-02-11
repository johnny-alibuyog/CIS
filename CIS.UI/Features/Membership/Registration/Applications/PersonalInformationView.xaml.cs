using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Registration.Applications;

/// <summary>
/// Interaction logic for PersonalInformationView.xaml
/// </summary>
public partial class PersonalInformationView : UserControl, IViewFor<PersonalInformationViewModel>
{
    #region IViewFor<PersonalInformationViewModel> Members

    public PersonalInformationViewModel ViewModel
    {
        get { return this.DataContext as PersonalInformationViewModel; }
        set { this.DataContext = value; }
    }

    object IViewFor.ViewModel
    {
        get { return this.DataContext; }
        set { this.DataContext = value; }
    }

    #endregion

    public PersonalInformationView()
    {
        InitializeComponent();
    }
}
