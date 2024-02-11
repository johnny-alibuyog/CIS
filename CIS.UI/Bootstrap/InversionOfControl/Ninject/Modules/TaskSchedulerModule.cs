using CIS.UI.Features;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules;

public class TaskSchedulerModule : NinjectModule
{
    public override void Load()
    {
        Kernel.Bind(x => x.FromThisAssembly()
            .SelectAllClasses()
            .InheritedFrom(typeof(ITaskScheduler))
            .BindAllInterfaces()
            .Configure(o => o.InSingletonScope()));
    }
}
