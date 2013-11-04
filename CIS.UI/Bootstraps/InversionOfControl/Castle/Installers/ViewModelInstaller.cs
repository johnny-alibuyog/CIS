using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CIS.UI.Features;

namespace CIS.UI.Bootstraps.InversionOfControl.Castle.Installers
{
    public class ViewModelInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<ViewModelBase>()
                    .Configure((ComponentRegistration config) =>
                    {
                        config.LifestyleTransient();
                        config.Named(config.Implementation.FullName);
                    })
            );
        }
    }
}
