using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.DependencyInjection.Ninject.Modules;
using Ninject;
using Ninject.Modules;

namespace CIS.UI.Bootstraps.DependencyInjection.Ninject
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

        public T Resolve<T>()
        {
            return _kernel.Get<T>();
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
                    kernel.Load(AppDomain.CurrentDomain.GetAssemblies());

                    _instance = new DependencyResolver(kernel);
                }
                return _instance;
            }
        }

        #endregion

    }
}
