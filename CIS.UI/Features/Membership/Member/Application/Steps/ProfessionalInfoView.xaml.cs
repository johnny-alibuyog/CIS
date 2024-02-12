using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Member.Application.Steps;
/// <summary>
/// Interaction logic for ProfessionalInfoView.xaml
/// </summary>
public partial class ProfessionalInfoView : UserControl, IViewFor<ProfessionalInfoViewModel>
{
    public ProfessionalInfoViewModel ViewModel
    {
        get => this.DataContext as ProfessionalInfoViewModel;
        set => this.DataContext = value;
    }

    object IViewFor.ViewModel
    {
        get => this.DataContext;
        set => this.DataContext = value;
    }

    public ProfessionalInfoView()
    {
        InitializeComponent();
    }
}
