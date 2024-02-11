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
    public class ImporterInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<IImportService>()
                    .Configure(config =>
                    {
                        config.LifestyleTransient();
                        config.Named(config.Implementation.FullName);
                    })
            );
        }
    }
}
