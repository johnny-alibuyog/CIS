using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Member.Application.Steps;
/// <summary>
/// Interaction logic for MembershipInfoView.xaml
/// </summary>
public partial class MembershipInfoView : UserControl, IViewFor<MembershipInfoViewModel>
{
    public MembershipInfoViewModel ViewModel
    {
        get => this.DataContext as MembershipInfoViewModel;
        set => this.DataContext = value;
    }

    object IViewFor.ViewModel
    {
        get => this.DataContext;
        set => this.DataContext = value;
    }

    public MembershipInfoView()
    {
        InitializeComponent();
    }
}
