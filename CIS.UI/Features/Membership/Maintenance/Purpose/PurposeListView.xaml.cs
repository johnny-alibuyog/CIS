using System.Windows.Controls;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Maintenance.Purpose;

/// <summary>
/// Interaction logic for PurposeListView.xaml
/// </summary>
public partial class PurposeListView : UserControl, IViewFor<PurposeListViewModel>
{
    #region IViewFor<PurposeViewModel> Members

    public PurposeListViewModel ViewModel
    {
        get { return this.DataContext as PurposeListViewModel; }
        set { this.DataContext = value; }
    }

    object IViewFor.ViewModel
    {
        get { return this.DataContext; }
        set { this.DataContext = value; }
    }
    #endregion

    public PurposeListView()
    {
        this.InitializeComponent();
        this.InitializeViewModel(() => IoC.Container.Resolve<PurposeListViewModel>());
    }
}
