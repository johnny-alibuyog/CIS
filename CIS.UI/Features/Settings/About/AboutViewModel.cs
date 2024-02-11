using CIS.UI.Bootstraps.InversionOfControl;

namespace CIS.UI.Features.Settings.About;

public class AboutViewModel : ViewModelBase
{
    private readonly AboutController _controller;

    public virtual string About { get; set; }

    public AboutViewModel()
    {
        _controller = IoC.Container.Resolve<AboutController>(new ViewModelDependency(this));
    }
}
