using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.DependencyInjection.Ninject;

namespace CIS.UI.Bootstraps.DependencyInjection
{
    public static class IoC
    {
        public static IDependencyResolver Container
        {
            get { return DependencyResolver.Instance; }
        }
    }
}
