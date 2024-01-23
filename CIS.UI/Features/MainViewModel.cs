using System;
using CIS.UI.Bootstraps.InversionOfControl;
using FirstFloor.ModernUI.Presentation;

namespace CIS.UI.Features;

public class MainViewModel : ViewModelBase
{
    private readonly MainController _controller;

    public virtual Uri DefaultPage { get; set; }
    public virtual LinkGroupCollection LinkGroups { get; set; }

    public MainViewModel()
    {
        _controller = IoC.Container.Resolve<MainController>(new ViewModelDependency(this));
    }
}
