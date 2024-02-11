using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Common.Camera;

/// <summary>
/// Interaction logic for CameraView.xaml
/// </summary>
public partial class CameraView : UserControl, IViewFor<CameraViewModel>
{
    #region IViewFor<CameraViewModel> Members

    public CameraViewModel ViewModel
    {
        get { return this.DataContext as CameraViewModel; }
        set { this.DataContext = value; }
    }

    object IViewFor.ViewModel
    {
        get { return this.DataContext; }
        set { this.DataContext = value; }
    }

    #endregion

    public CameraView()
    {
        InitializeComponent();

        //DataContext = new CameraViewModel();
    }
}
