using Ninject.Modules;
using ServiceStack;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules
{
    public class DataStoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRestClient>().ToMethod(x =>
            {
                return new JsonServiceClient() // XmlServiceClient, JsvServiceClient, MsgPackServiceClient
                {
                    BaseUri = App.Context.DataStore.BaseUri,
                    UserName = App.Context.DataStore.Username,
                    Password = App.Context.DataStore.Password
                };
            });

            //Bind<IServiceClient>()
            //    .To<Soap11ServiceClient>();
        }
    }
}
