using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace CIS.UI.Bootstraps.InversionOfControl.Castle
{
    public class DependencyResolver : IDependencyResolver
    {
        WindsorContainer _container ;

        private DependencyResolver(WindsorContainer container)
        {
            _container = container;
        }
                
        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public object Resolve(Type type, params Dependency[] dependencies)
        {
            var args = dependencies.ToDictionary(x => x.Name, x => x.Value);
            return _container.Resolve(type, args);
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(params Dependency[] dependencies)
        {
            var args = dependencies.ToDictionary(x => x.Name, x => x.Value);
            return _container.Resolve<T>(args);
        }

        #region Static Members

        private static IDependencyResolver _instance;

        public static IDependencyResolver Instance
        {
            get
            {
                if (_instance == null)
                {
                    var container = new WindsorContainer();
                    container.Install(FromAssembly.Containing<Installers.AttributeInstaller>());
                    container.Register(Types.FromThisAssembly());
                    _instance = new DependencyResolver(container);
                }
                return _instance;
            }
        }

        #endregion
    }
}
