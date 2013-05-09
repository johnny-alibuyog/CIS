using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Utilities.Extentions
{
    public static class EnumExtention
    {
        public static T As<T>(this string value) where T : struct
        {
            T result;

            Enum.TryParse<T>(value, true, out result);

            return result;
        }
    }
}
