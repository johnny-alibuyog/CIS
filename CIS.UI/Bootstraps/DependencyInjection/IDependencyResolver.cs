using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Bootstraps.DependencyInjection
{
    public interface IDependencyResolver
    {
        object Resolve(Type type);
        T Resolve<T>();
    }
}
