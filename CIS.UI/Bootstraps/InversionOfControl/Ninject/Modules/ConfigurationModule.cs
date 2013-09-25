using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Utilities.Configurations;
using Ninject.Modules;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules
{
    public class ConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ApplicationConfiguration>().ToSelf()
                .InSingletonScope();
        }
    }
}
