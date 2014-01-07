using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CIS.Store.Domain.Entities;
using CIS.Store.Properties;
using CIS.Store.Services.Warrants;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace CIS.Store
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("CIS Store", typeof(WarrantService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            this.RegisterServices(container);
            this.ConfigureDataBase(container);

            JsConfig.EmitCamelCaseNames = true;
        }

        private void RegisterServices(Funq.Container container)
        {
            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                connectionString: Settings.Default.ConnectionString,
                dialectProvider: SqlServerDialect.Provider
            ));
        }

        private void ConfigureDataBase(Funq.Container container)
        {
            using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            using (var transaction = db.BeginTransaction())
            {
                //db.CreateTable<Warrant>(overwrite: true);
                db.CreateTable<Warrant>();
                db.CreateTable<AutditTrail>();

                transaction.Commit();
            }
        }
    }
}