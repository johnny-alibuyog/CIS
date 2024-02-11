using Ninject.Extensions.Conventions;
using Ninject.Modules;
using ReactiveUI;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules;

public class ViewModule : NinjectModule
{
    public override void Load()
    {
        Kernel.Bind(x => x.FromThisAssembly()
            .SelectAllClasses()
            .InheritedFrom(typeof(IViewFor<>))
            .BindAllInterfaces());
    }
}
