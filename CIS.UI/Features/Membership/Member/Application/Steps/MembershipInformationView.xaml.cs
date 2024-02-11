using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Member.Application.Steps;
/// <summary>
/// Interaction logic for PersonalInformationView.xaml
/// </summary>
public partial class MembershipInformationView : UserControl, IViewFor<MembershipInformationViewModel>
{
    public MembershipInformationViewModel ViewModel
    {
        get => this.DataContext as MembershipInformationViewModel;
        set => this.DataContext = value;
    }

    object IViewFor.ViewModel
    {
        get => this.DataContext;
        set => this.DataContext = value;
    }

    public MembershipInformationView()
    {
        InitializeComponent();
    }
}
