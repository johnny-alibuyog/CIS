using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CIS.Core.Entities.Memberships;

namespace CIS.UI.Bootstraps.InversionOfControl.Castle.Installers
{
    public class AttributeInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(
            //    Classes.FromThisAssembly()
            //        .BasedOn<Attribute>()
            //        .LifestyleTransient()
            //);
        }
    }
}
