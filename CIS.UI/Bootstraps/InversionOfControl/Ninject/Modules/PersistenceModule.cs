using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Data;
using CIS.Data.Configurations;
using CIS.UI.Utilities;
using NHibernate;
using NHibernate.Context;
using NHibernate.Validator.Engine;
using Ninject.Modules;
using Ninject.Extensions.Interception;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules
{
    public class PersistenceModule : NinjectModule
    {
        public override void Load()
        {
            SessionProvider.Instance.AuditResolver = new UserAuditResolver();

            Bind<ISessionProvider>()
                .ToMethod(x => SessionProvider.Instance)
                .InSingletonScope();

            Bind<ISessionFactory>()
                .ToMethod(x => SessionProvider.Instance.SessionFactory)
                .InSingletonScope();

            Bind<ValidatorEngine>()
                .ToMethod(x => SessionProvider.Instance.ValidatorEngine)
                .InSingletonScope();

            Bind<AuditResolver>()
                .ToMethod(x => SessionProvider.Instance.AuditResolver)
                .InSingletonScope();
        }
    }
}
