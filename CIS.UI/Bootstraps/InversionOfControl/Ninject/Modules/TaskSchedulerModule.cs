
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Features;
using Ninject;
using Ninject.Extensions;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules
{
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
}
