using Ninject.Modules;
using ReactiveUI;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules
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
