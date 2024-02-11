using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Registration.Applications;

/// <summary>
/// Interaction logic for SummaryView.xaml
/// </summary>
public partial class SummaryView : UserControl, IViewFor<SummaryViewModel>
{
    #region IViewFor<SummaryViewModel> Members

    public SummaryViewModel ViewModel
    {
        get { return this.DataContext as SummaryViewModel; }
        set { this.DataContext = value; }
    }

    object IViewFor.ViewModel
    {
        get { return this.DataContext; }
        set { this.DataContext = value; }
    }
    #endregion

    public SummaryView()
    {
        InitializeComponent();
    }
}
