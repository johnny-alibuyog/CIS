using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl.Ninject;

namespace CIS.UI.Bootstraps.InversionOfControl
{
    public static class IoC
    {
        public static IDependencyResolver Container
        {
            get { return DependencyResolver.Instance; }
        }
    }
}
