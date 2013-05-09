using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using ReactiveUI;

namespace CIS.UI.Bootstraps.DependencyInjection.Ninject.Modules
{
    public class MessageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageBus>()
                .To<MessageBus>()
                .InSingletonScope();
        }
    }
}
