using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Bootstraps.InversionOfControl
{
    public class Dependency
    {
        public virtual string Name { get; private set; }

        public virtual object Value { get; private set; }

        public Dependency(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
