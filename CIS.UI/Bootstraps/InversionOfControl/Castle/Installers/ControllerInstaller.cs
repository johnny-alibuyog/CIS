using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CIS.UI.Bootstraps.InversionOfControl.Castle.Interceptors;
using CIS.UI.Features;

namespace CIS.UI.Bootstraps.InversionOfControl.Castle.Installers
{
    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn(typeof(ControllerBase<>))
                    .Configure((ComponentRegistration config) =>
                    {
                        //config.Interceptors<AuthorizeInterceptor>()
                        //    .SelectInterceptorsWith(new AttributeInterceptorSelector());

                        //var temp = config
                        //    .Interceptors(
                        //        InterceptorReference.ForType<AuthorizeInterceptor>(),
                        //        InterceptorReference.ForType<HandleErrorInterceptor>()
                        //    )
                        //    .SelectedWith(new AttributeInterceptorSelector()).Anywhere;

                        config.LifestyleTransient()
                            .Interceptors(
                                InterceptorReference.ForType<AuthorizeInterceptor>(),
                                InterceptorReference.ForType<HandleErrorInterceptor>()
                            )
                            .SelectedWith(new AttributeInterceptorSelector());

                        config.Named(config.Implementation.FullName);
                    })
            );
        }
    }
}
