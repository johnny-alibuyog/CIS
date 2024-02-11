using CIS.UI.Utilities.Configurations;
using Ninject.Modules;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules
{
    public class ConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ApplicationConfiguration>()
                .ToMethod(x =>
                {
                    var appConfig = new ApplicationConfiguration();
                    appConfig.Initialize();
                    return appConfig;
                })
                .InSingletonScope();
        }
    }
}
