using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Memberships;
using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;
using Ninject.Parameters;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors
{
    //public class AuthorizeAttribute : Attribute
    //{
    //    public virtual Role[] Roles { get; set; }
    //}

    public class AuthorizeAttribute : InterceptAttribute
    {
        public virtual Role[] Roles { get; set; }

        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            //return IoC.Container.Resolve<AuthorizeInterceptor>(new Dependency("roles", Roles));

            return request.Context.Kernel.Get<AuthorizeInterceptor>(new ConstructorArgument("roles", Roles));
        }
    }
}
