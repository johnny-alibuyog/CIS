using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules;
using Ninject;
using Ninject.Modules;
using Ninject.Extensions;
using Ninject.Extensions.Interception;
using Ninject.Planning.Strategies;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using System.Reflection;
using Ninject.Parameters;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject
{
    public sealed class DependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        private DependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public object Resolve(Type type)
        {
            return _kernel.Get(type);
        }

        public object Resolve(Type type, params Dependency[] dependencies)
        {
            var parameters = dependencies.Select(x => new ConstructorArgument(x.Name, x.Value)).ToArray();
            return _kernel.Get(type, parameters);
        }

        public T Resolve<T>()
        {
            return _kernel.Get<T>();
        }

        public T Resolve<T>(params Dependency[] dependencies)
        {
            var parameters = dependencies.Select(x => new ConstructorArgument(x.Name, x.Value)).ToArray();
            return _kernel.Get<T>(parameters);
        }

        #region Static Members

        private static IDependencyResolver _instance;

        public static IDependencyResolver Instance
        {
            get
            {
                if (_instance == null)
                {
                    var kernel = new StandardKernel();
                    //kernel.Load(AppDomain.CurrentDomain.GetAssemblies());
                    kernel.Load(Assembly.GetExecutingAssembly());
                    kernel.Components.Add<IPlanningStrategy, CustomPlanningStrategy<AuthorizeAttribute, AuthorizeInterceptor>>();
                    kernel.Components.Add<IPlanningStrategy, CustomPlanningStrategy<HandleErrorAttribute, HandleErrorInterceptor>>();

                    _instance = new DependencyResolver(kernel);
                }
                return _instance;
            }
        }

        #endregion

    }
}
