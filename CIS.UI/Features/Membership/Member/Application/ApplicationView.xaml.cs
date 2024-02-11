using System.Windows.Controls;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Member.Application;

/// <summary>
/// Interaction logic for ApplicationView.xaml
/// </summary>
public partial class ApplicationView : UserControl, IViewFor<ApplicationViewModel>
{
    public ApplicationViewModel ViewModel
    {
        get { return this.DataContext as ApplicationViewModel; }
        set { this.DataContext = value; }
    }

    object IViewFor.ViewModel
    {
        get { return this.DataContext; }
        set { this.DataContext = value; }
    }

    public ApplicationView()
    {
        this.InitializeComponent();
        this.InitializeViewModel(() => new());
    }
}
