using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Common.Camera;

/// <summary>
/// Interaction logic for CameraDialogView.xaml
/// </summary>
public partial class CameraDialogView : DialogBase, IViewFor<CameraDialogViewModel>
{
    #region IViewFor<CameraDialogViewModel> Members

    public CameraDialogViewModel ViewModel
    {
        get { return this.DataContext as CameraDialogViewModel; }
        set { this.DataContext = value; }
    }

    object IViewFor.ViewModel
    {
        get { return this.DataContext; }
        set { this.DataContext = value; }
    }

    #endregion

    public CameraDialogView()
    {
        this.InitializeComponent();
        this.InitializeViewModel(() => IoC.Container.Resolve<CameraDialogViewModel>());

        this.Loaded += (sender, e) => this.ViewModel.Camera.Start.Execute(null);
        this.Unloaded += (sender, e) => this.ViewModel.Camera.Stop.Execute(null);
    }
}
