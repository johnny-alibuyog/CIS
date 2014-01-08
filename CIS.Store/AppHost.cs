using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CIS.Store.Domain.Entities;
using CIS.Store.Properties;
using CIS.Store.Services.Warrants;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Caching;
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
            this.ConfigureDataBase(container);
            this.ConfigureAuthentication(container);

            JsConfig.EmitCamelCaseNames = true;
        }

        private void ConfigureDataBase(Funq.Container container)
        {
            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                connectionString: Settings.Default.ConnectionString,
                dialectProvider: SqlServerDialect.Provider
            ));

            using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            using (var transaction = db.BeginTransaction())
            {
                //db.CreateTable<Warrant>(overwrite: true);
                db.CreateTable<Warrant>();
                db.CreateTable<AuditTrail>();

                transaction.Commit();
            }
        }

        private void ConfigureAuthentication(Funq.Container container)
        {
            this.Plugins.Add(new AuthFeature(
                sessionFactory: () => new AuthUserSession(),
                authProviders: new IAuthProvider[] 
                { 
                    new BasicAuthProvider(),        //Sign-in with Basic Auth
                    new CredentialsAuthProvider(),  //HTML Form post of UserName/Password credentials
                })
            );

            this.Plugins.Add(new RegistrationFeature());

            container.Register<ICacheClient>(new MemoryCacheClient());
            container.Register<IUserAuthRepository>(new OrmLiteAuthRepository(container.Resolve<IDbConnectionFactory>()));
            //container.Register<IUserAuthRepository>(new InMemoryAuthRepository());

            var repository = container.Resolve<IUserAuthRepository>();
            repository.InitSchema();

            var user = repository.GetUserAuthByUserName(Settings.Default.DefaultUserName);
            if (user == null)
            {
                var hash = (string)null;
                var salt = (string)null;
                new SaltedHash().GetHashAndSaltString(Settings.Default.DefaultUserPassword, out hash, out salt);

                user = new UserAuth()
                {
                    UserName = Settings.Default.DefaultUserName,
                    PasswordHash = hash,
                    Salt = salt
                };

                repository.CreateUserAuth(user, Settings.Default.DefaultUserPassword);
            }
        }
    }
}