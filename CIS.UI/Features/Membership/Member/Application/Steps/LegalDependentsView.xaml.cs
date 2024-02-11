using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Member.Application.Steps;

/// <summary>
/// Interaction logic for LegalDependentsView.xaml
/// </summary>
public partial class LegalDependentsView : UserControl, IViewFor<LegalDependentsViewModel>
{
    public LegalDependentsViewModel ViewModel
    {
        get => this.DataContext as LegalDependentsViewModel;
        set => this.DataContext = value;
    }

    object IViewFor.ViewModel
    {
        get => this.DataContext;
        set => this.DataContext = value;
    }

    public LegalDependentsView()
    {
        InitializeComponent();
    }
}
