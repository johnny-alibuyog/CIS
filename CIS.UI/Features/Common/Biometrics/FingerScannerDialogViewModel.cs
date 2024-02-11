using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Common.Biometrics;

public class FingerScannerDialogViewModel : ViewModelBase
{
    private readonly FingerScannerDialogController _controller;

    [Valid]
    public virtual FingerScannerViewModel FingerScanner { get; set; }

    public virtual IReactiveCommand Accept { get; set; }

    public FingerScannerDialogViewModel()
    {
        FingerScanner = new FingerScannerViewModel();

        //_controller = new SignatureDialogController(this);
        _controller = IoC.Container.Resolve<FingerScannerDialogController>(new ViewModelDependency(this));
    }
}
