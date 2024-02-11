using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules;
using Ninject;
using Ninject.Modules;
using Ninject.Parameters;
using Ninject.Planning.Strategies;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject
{
    public sealed class DependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        private DependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public object Resolve(Type type, params Dependency[] dependencies)
        {
            if (dependencies != null && dependencies.Count() > 0)
            {
                var parameters = dependencies.Select(x => new ConstructorArgument(x.Name, x.Value)).ToArray();
                return _kernel.Get(type, parameters);
            }
            else
            {
                return _kernel.Get(type);
            }
        }

        public T Resolve<T>(params Dependency[] dependencies)
        {
            if (dependencies != null && dependencies.Count() > 0)
            {
                var parameters = dependencies.Select(x => new ConstructorArgument(x.Name, x.Value)).ToArray();
                return _kernel.Get<T>(parameters);
            }
            else
            {
                return _kernel.Get<T>();
            }

        }

        public IEnumerable<object> ResolveAll(Type type, params Dependency[] dependencies)
        {
            if (dependencies != null && dependencies.Count() > 0)
            {
                var parameters = dependencies.Select(x => new ConstructorArgument(x.Name, x.Value)).ToArray();
                return _kernel.GetAll(type, parameters);
            }
            else
            {
                return _kernel.GetAll(type);
            }
        }

        public IEnumerable<T> ResolveAll<T>(params Dependency[] dependencies)
        {
            if (dependencies != null && dependencies.Count() > 0)
            {
                var parameters = dependencies.Select(x => new ConstructorArgument(x.Name, x.Value)).ToArray();
                return _kernel.GetAll<T>(parameters);
            }
            else
            {
                return _kernel.GetAll<T>();
            }

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
                    //kernel.Components.Add<IPlanningStrategy, CustomPlanningStrategy<AuthorizeAttribute, AuthorizeInterceptor>>();
                    //kernel.Components.Add<IPlanningStrategy, CustomPlanningStrategy<HandleErrorAttribute, HandleErrorInterceptor>>();

                    _instance = new DependencyResolver(kernel);
                }
                return _instance;
            }
        }

        #endregion
    }
}
