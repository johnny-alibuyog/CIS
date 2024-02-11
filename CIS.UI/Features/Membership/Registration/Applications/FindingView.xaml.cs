using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Registration.Applications;

/// <summary>
/// Interaction logic for FindingView.xaml
/// </summary>
public partial class FindingView : UserControl, IViewFor<FindingViewModel>
{
    public FindingViewModel ViewModel
    {
        get => this.DataContext as FindingViewModel;
        set => this.DataContext = value;
    }

    object IViewFor.ViewModel
    {
        get => this.DataContext;
        set => this.DataContext = value;
    }

    public FindingView()
    {
        InitializeComponent();
    }
}
