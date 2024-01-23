using System.Collections.Generic;
using CIS.UI.Bootstraps.InversionOfControl;

namespace CIS.UI.Features;

public class SplashScreenViewModel : ViewModelBase
{
    private readonly SplashScreenController _controller;

    public virtual string Licensee { get; set; }

    public virtual IEnumerable<string> Plugins { get; set; }

    public virtual string Message { get; set; } 

    public SplashScreenViewModel()
    {
        this.Message = "Loading";
        _controller = IoC.Container.Resolve<SplashScreenController>(new ViewModelDependency(this));
    } 

}
