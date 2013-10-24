using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Bootstraps.InversionOfControl
{
    public static class IoC
    {
        public static IDependencyResolver Container
        {
            //get { return Castle.DependencyResolver.Instance; }
            get { return Ninject.DependencyResolver.Instance; }
        }
    }
}
