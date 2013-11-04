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
    public class AttributeInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            Predicate<Type> DoesNotHave = (attributeType) => Attribute.GetCustomAttribute(method, attributeType, false) == null;

            var query = interceptors.AsEnumerable();

            if (DoesNotHave(typeof(AuthorizeAttribute)))
                query = query.Where(x => !(x is AuthorizeInterceptor));

            if (DoesNotHave(typeof(HandleErrorAttribute)))
                query = query.Where(x => !(x is HandleErrorInterceptor));

            return query.ToArray();
        }
    }
}
