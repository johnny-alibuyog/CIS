using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Bootstraps.InversionOfControl
{
    public interface IDependencyResolver
    {
        object Resolve(Type type, params Dependency[] dependencies);
        T Resolve<T>(params Dependency[] dependencies);
        IEnumerable<object> ResolveAll(Type type, params Dependency[] dependencies);
        IEnumerable<T> ResolveAll<T>(params Dependency[] dependencies);
    }
}
