using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CIS.UI.Features;
using ReactiveUI;

namespace CIS.UI.Bootstraps.InversionOfControl.Castle.Installers
{
    public class ViewInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<Window>()
                    .OrBasedOn(typeof(IViewFor<>))
                    .Configure((ComponentRegistration config) =>
                    {
                        config.LifestyleTransient();
                        config.Named(config.Implementation.FullName);
                    })
            );
        }
    }
}
