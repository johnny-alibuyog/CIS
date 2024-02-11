using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using CIS.UI.Bootstraps.InversionOfControl.Attributes;

namespace CIS.UI.Bootstraps.InversionOfControl.Castle.Interceptors
{
    public class HandleErrorInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var authenticate = Attribute.GetCustomAttribute(method, typeof(HandleErrorAttribute), false) as HandleErrorAttribute;
            if (authenticate == null)
                return interceptors.Where(x => !(x is HandleErrorAttribute)).ToArray();

            return interceptors;
        }
    }
}
