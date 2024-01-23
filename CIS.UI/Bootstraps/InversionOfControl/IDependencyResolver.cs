using System;
using System.Collections.Generic;

namespace CIS.UI.Bootstraps.InversionOfControl;

public interface IDependencyResolver
{
    object Resolve(Type type, params Dependency[] dependencies);
    T Resolve<T>(params Dependency[] dependencies);
    IEnumerable<object> ResolveAll(Type type, params Dependency[] dependencies);
    IEnumerable<T> ResolveAll<T>(params Dependency[] dependencies);
}
