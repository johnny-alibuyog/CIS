using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Bootstraps.InversionOfControl
{
    public class ViewModelDependency : Dependency
    {
        public ViewModelDependency(object value) : base("viewModel", value) { }
    }
}
