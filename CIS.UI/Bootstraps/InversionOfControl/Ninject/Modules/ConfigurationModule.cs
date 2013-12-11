using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Utilities.Extentions;
using CIS.UI.Utilities.Configurations;
using Ninject.Modules;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules
{
    public class ConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ApplicationConfiguration>()//.ToSelf()
                .ToMethod(x =>
                {
                    var appConfig = new ApplicationConfiguration();
                    appConfig.Initialize();

                    StringExtention.SetProperCaseSuffix(appConfig.ProperCasing.Suffixes);
                    StringExtention.SetProperCaseSpecialWords(appConfig.ProperCasing.SpecialWords);

                    return appConfig;
                })
                .InSingletonScope();
        }
    }
}
