using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CIS.Core.Entities.Commons;
using CIS.Data;
using CIS.Data.Configurations;
using CIS.UI.Utilities;
using CIS.UI.Utilities.Configurations;
using NHibernate;
using NHibernate.Validator.Engine;

namespace CIS.UI.Bootstraps.InversionOfControl.Castle.Installers
{
    public class PersistenceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromAssemblyContaining<Audit>().Pick()
                    .Configure(config =>
                    {
                        config.LifestyleTransient();
                        config.Named(config.Implementation.FullName);
                    })
            );

            container.Register(
                Component.For<AuditResolver>()
                    .ImplementedBy<UserAuditResolver>()
                    .LifestyleSingleton()
            );

            container.Register(
                Component.For<ValidatorEngine>()
                    .ImplementedBy<ValidatorEngine>()
                    .LifestyleSingleton()
            );

            container.Register(
                Component.For<ISessionProvider>()
                    .UsingFactoryMethod(x =>
                    {
                        var configuration = x.Resolve<ApplicationConfiguration>();
                        return new SessionProvider(
                            validator: x.Resolve<ValidatorEngine>(),
                            auditResolver: x.Resolve<AuditResolver>(),
                            serverName: configuration.Database.ServerName,
                            databaseName: configuration.Database.DatabaseName,
                            username: configuration.Database.Username,
                            password: configuration.Database.Password
                        );
                    })
                    .LifestyleSingleton()
            );

            container.Register(
                Component.For<ISessionFactory>()
                    .UsingFactoryMethod(x => x.Resolve<ISessionProvider>().SessionFactory)
                    .LifestyleSingleton()
            );
        }
    }
}
