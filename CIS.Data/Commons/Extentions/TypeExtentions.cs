using System;
using System.Linq;

namespace CIS.Data.Commons.Extentions;

public static class TypeExtentions
{
    public static string ParseSchema(this Type type)
    {
        return type.Namespace.Split('.').Last();
    }
}
