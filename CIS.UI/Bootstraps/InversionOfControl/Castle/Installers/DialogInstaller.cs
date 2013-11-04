using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CIS.UI.Utilities.CommonDialogs;

namespace CIS.UI.Bootstraps.InversionOfControl.Castle.Installers
{
    public class DialogInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IMessageBoxService>()
                    .ImplementedBy<MessageBoxService>()
                    .LifestyleTransient()
            );

            container.Register(
                Component.For<IOpenDirectoryDialogService>()
                    .ImplementedBy<OpenDirectoryDialogService>()
                    .LifestyleTransient()
            );

            container.Register(
                Component.For<IOpenImageDialogService>()
                    .ImplementedBy<OpenImageDialogService>()
                    .LifestyleTransient()
            );
        }
    }
}
