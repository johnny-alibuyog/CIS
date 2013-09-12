using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Bootstraps.InversionOfControl
{
    public interface IDependecyParameter
    {
        string Name { get; }
        object Value { get; }
    }
}
