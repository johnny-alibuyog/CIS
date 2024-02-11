using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CIS.UI.Features.Common.Address
{
    public class ProvinceLookup : Lookup<Guid>
    {
        public virtual IReactiveList<CityLookup> Cities { get; set; }
    }
}
