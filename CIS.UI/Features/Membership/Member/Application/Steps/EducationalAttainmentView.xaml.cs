using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Member.Application.Steps;
/// <summary>
/// Interaction logic for PersonalInformationView.xaml
/// </summary>
public partial class EducationalAttainmentView : UserControl, IViewFor<EducationalAttainmentViewModel>
{
    public EducationalAttainmentViewModel ViewModel
    {
        get => this.DataContext as EducationalAttainmentViewModel;
        set => this.DataContext = value;
    }

    object IViewFor.ViewModel
    {
        get => this.DataContext;
        set => this.DataContext = value;
    }

    public EducationalAttainmentView()
    {
        InitializeComponent();
    }
}
