using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CIS.Data.Configurations;
using CIS.Data.Conventions;
using CIS.Data.EntityDefinition.Commons;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Context;
using NHibernate.Validator.Engine;

namespace CIS.Data
{
    public class SessionProvider : ISessionProvider
    {
        private AuditResolver _auditResolver;
        private readonly ISessionFactory _sessionFactory;
        private static ISessionProvider _instance = new SessionProvider();

        public static ISessionProvider Instance
        {
            get { return _instance; }
        }

        public virtual ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        public virtual ValidatorEngine ValidatorEngine
        {
            get { return ValidatorConfiguration.ValidatorEngine; }
        }

        public virtual AuditResolver AuditResolver
        {
            get { return _auditResolver; }
            set { _auditResolver = value; }
        }

        public virtual ISession GetSharedSession()
        {
            var session = (ISession)null;
            var sessionFactory = SessionFactory;
            if (CurrentSessionContext.HasBind(sessionFactory))
            {
                session = sessionFactory.GetCurrentSession();
            }
            else
            {
                session = sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }
            return session;
        }

        public virtual ISession ReleaseSharedSession()
        {
            return CurrentSessionContext.Unbind(SessionFactory);
        }

        private ISessionFactory CreateSessionFactory()
        {
            var schemaExportPath = Path.Combine(System.Environment.CurrentDirectory, "Mappings");

            if (!Directory.Exists(schemaExportPath))
                Directory.CreateDirectory(schemaExportPath);

            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                    .DefaultSchema("dbo")
                    .ConnectionString(x => x.FromConnectionStringWithKey("ConnectionString"))
                    .QuerySubstitutions("true 1, false 0, yes y, no n")
                    .AdoNetBatchSize(15)
                    .FormatSql()
                    //.ShowSql()
                )
                .Mappings(x => x
                    .FluentMappings.AddFromAssemblyOf<AuditMapping>()
                    .Conventions.AddFromAssemblyOf<_CustomJoinedSubclassConvention>()
                    .Conventions.Setup(o => o.Add(AutoImport.Never()))
                    .ExportTo(schemaExportPath)
                )
                .ProxyFactoryFactory<DefaultProxyFactoryFactory>()
                .ExposeConfiguration(EventListenerConfiguration.Configure)
                .ExposeConfiguration(CacheConfiguration.Configure)
                .ExposeConfiguration(ValidatorConfiguration.Configure)
                .ExposeConfiguration(IndexForeignKeyConfiguration.Configure)
                .ExposeConfiguration(SchemaConfiguration.Configure)
                .ExposeConfiguration(CurrentSessionContextConfiguration.Configure)
                .BuildSessionFactory();
        }

        private SessionProvider()
        {
             _sessionFactory = CreateSessionFactory();
        }
    }
}
