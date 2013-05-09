using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Data.Commons.Extentions
{
    public static class TypeExtentions
    {
        public static string ParseSchema(this Type type)
        {
            return type.Namespace.Split('.').Last();
        }
    }
}
