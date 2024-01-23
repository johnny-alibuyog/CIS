using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Commons.Cameras;

public class CameraDialogViewModel : ViewModelBase
{
    private readonly CameraDialogController _controller;

    [Valid]
    public virtual CameraViewModel Camera { get; set; }

    public virtual IReactiveCommand Accept { get; set; }

    public CameraDialogViewModel()
    {
        Camera = new CameraViewModel();

        _controller = IoC.Container.Resolve<CameraDialogController>(new ViewModelDependency(this));
    }
}
