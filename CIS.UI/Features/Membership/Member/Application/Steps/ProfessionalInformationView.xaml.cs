using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Member.Application.Steps;
/// <summary>
/// Interaction logic for ProfessionalInformationView.xaml
/// </summary>
public partial class ProfessionalInformationView : UserControl, IViewFor<ProfessionalInformationViewModel>
{
    public ProfessionalInformationViewModel ViewModel
    {
        get => this.DataContext as ProfessionalInformationViewModel;
        set => this.DataContext = value;
    }

    object IViewFor.ViewModel
    {
        get => this.DataContext;
        set => this.DataContext = value;
    }

    public ProfessionalInformationView()
    {
        InitializeComponent();
    }
}
