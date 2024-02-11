using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Registration.Applications;

/// <summary>
/// Interaction logic for OtherInformationView.xaml
/// </summary>
public partial class OtherInformationView : UserControl, IViewFor<OtherInformationViewModel>
{
    public OtherInformationViewModel ViewModel
    {
        get => this.DataContext as OtherInformationViewModel;
        set => this.DataContext = value;
    }

    object IViewFor.ViewModel
    {
        get => this.DataContext;
        set => this.DataContext = value;
    }

    public OtherInformationView()
    {
        InitializeComponent();
    }
}
