using System;
using System.Linq;

namespace CIS.Data.Common.Extention;

public static class TypeExtentions
{
    public static string ParseSchema(this Type type)
    {
        return type.Namespace.Split('.').Last();
    }
}
