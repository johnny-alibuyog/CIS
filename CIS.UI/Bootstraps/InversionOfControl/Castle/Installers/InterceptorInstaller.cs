using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CIS.UI.Bootstraps.InversionOfControl.Castle.Interceptors;

namespace CIS.UI.Bootstraps.InversionOfControl.Castle.Installers
{
    public class InterceptorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<AuthorizeInterceptor>()
                    .ImplementedBy<AuthorizeInterceptor>()
                    .LifestyleTransient()
            );

            container.Register(
                Component.For<HandleErrorInterceptor>()
                    .ImplementedBy<HandleErrorInterceptor>()
                    .LifestyleTransient()
            );

            container.Register(
                Component.For<AttributeInterceptorSelector>()
                    .ImplementedBy<AttributeInterceptorSelector>()
                    .LifestyleTransient()
            );
        }
    }
}
