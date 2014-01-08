using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Utilities.Configurations;
using Ninject;
using Ninject.Modules;
using ServiceStack;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules
{
    public class DataStoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRestClient>()
                .ToMethod(x =>
                {
                    return new JsonServiceClient() // XmlServiceClient, JsvServiceClient, MsgPackServiceClient
                    {
                        BaseUri = App.Data.DataStore.BaseUri,
                        UserName = App.Data.DataStore.Username,
                        Password = App.Data.DataStore.Password
                    };
                });

            //Bind<IServiceClient>()
            //    .To<Soap11ServiceClient>();
        }
    }
}
