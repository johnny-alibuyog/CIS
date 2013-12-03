using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
//using Ninject.Extensions.Interception;
//using Ninject.Extensions.Interception.Attributes;
//using Ninject.Extensions.Interception.Request;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors
{
    public class HandleErrorAttribute : Attribute { }

    //public class HandleErrorAttribute : InterceptAttribute
    //{
    //    public override IInterceptor CreateInterceptor(IProxyRequest request)
    //    {
    //        return request.Context.Kernel.Get<HandleErrorInterceptor>();
    //    }
    //}
}
