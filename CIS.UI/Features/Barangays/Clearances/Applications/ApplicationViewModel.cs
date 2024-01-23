using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Commons.Biometrics;
using CIS.UI.Features.Commons.Cameras;
using CIS.UI.Features.Commons.Signatures;
using ReactiveUI;
using System.Collections.Generic;

namespace CIS.UI.Features.Barangays.Clearances.Applications;

public class ApplicationViewModel : ViewModelBase
{
    private readonly ApplicationController _controller;

    public virtual PersonalInformationViewModel PersonalInformation { get; set; }

    public virtual CameraViewModel Camera { get; set; }

    public virtual FingerScannerViewModel FingerScanner { get; set; }

    public virtual SignatureViewModel Signature { get; set; }

    public virtual FindingViewModel Finding { get; set; }

    public virtual SummaryViewModel Summary { get; set; }

    public virtual IList<ViewModelBase> ViewModels { get; set; }

    public virtual ViewModelBase CurrentViewModel { get; set; }

    public virtual IReactiveCommand Previous { get; set; }

    public virtual IReactiveCommand Next { get; set; }

    public virtual IReactiveCommand Reset { get; set; }

    public virtual IReactiveCommand Release { get; set; }

    public ApplicationViewModel()
    {
        _controller = IoC.Container.Resolve<ApplicationController>(new ViewModelDependency(this));
    }
}
