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
        public static ReactiveCollection<T> ToReactiveColletion<T>(this IEnumerable<T> items)
        {
            return new ReactiveCollection<T>(items);
        }
    }
}
