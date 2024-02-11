using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Features.Memberships.Users;
using Ninject;
using Ninject.Modules;
//using Ninject.Extensions;
//using Ninject.Extensions.Interception;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules
{
    public class ControllerModule : NinjectModule
    {
        public override void Load()
        {
            //var binding = Kernel.Bind<UserListController>().To<UserListController>().Intercept;
            //Kernel.
        }
    }
}
