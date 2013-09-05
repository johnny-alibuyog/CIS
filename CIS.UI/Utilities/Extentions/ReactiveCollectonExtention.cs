using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CIS.UI.Utilities.Extentions
{
    public static class ReactiveCollectonExtention
    {
        public static IReactiveList<T> ToReactiveList<T>(this IEnumerable<T> items)
        {
            return new ReactiveList<T>(items);
        }
    }
}
