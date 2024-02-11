using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Common.Wizard;

namespace CIS.UI.Features.Membership.Member.Application;

public class ApplicationViewModel : ViewModelBase
{
    private readonly ApplicationController _controller;

    public string Title { get; set; } = "Application Title";

    public WizardViewModel Wizard { get; set; }

    public ApplicationViewModel()
    {
        //_controller = IoC.Container.Resolve<ApplicationController>(new ViewModelDependency(this));
        _controller = new ApplicationController(this);
    }
}
