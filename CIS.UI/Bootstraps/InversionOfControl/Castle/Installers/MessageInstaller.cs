using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ReactiveUI;

namespace CIS.UI.Bootstraps.InversionOfControl.Castle.Installers
{
    public class MessageInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IMessageBus>()
                    .ImplementedBy<MessageBus>()
                    .LifestyleSingleton()
            );
        }
    }
}
