﻿using CIS.Data;
using CIS.Data.Configurations;
using CIS.UI.Utilities;
using CIS.UI.Utilities.Configurations;
using NHibernate;
using NHibernate.Validator.Engine;
using Ninject;
using Ninject.Modules;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules;

public class PersistenceModule : NinjectModule
{
    public override void Load()
    {
        Bind<AuditResolver>()
            .ToMethod(x => new UserAuditResolver())
            .InSingletonScope();

        Bind<ValidatorEngine>()
            .ToMethod(x => new ValidatorEngine())
            .InSingletonScope();

        Bind<ISessionProvider>()
            .ToMethod(x =>
            {
                var configuration = x.Kernel.Get<ApplicationConfiguration>();
                return new SessionProvider(
                    validator: x.Kernel.Get<ValidatorEngine>(),
                    auditResolver: x.Kernel.Get<AuditResolver>(),
                    serverName: configuration.Database.ServerName,
                    databaseName: configuration.Database.DatabaseName,
                    username: configuration.Database.Username,
                    password: configuration.Database.Password
                );
            })
            .InSingletonScope();

        Bind<ISessionFactory>()
            .ToMethod(x => x.Kernel.Get<ISessionProvider>().SessionFactory)
            .InSingletonScope();
    }
}
