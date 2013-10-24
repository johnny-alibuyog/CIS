using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CIS.UI.Features.Commons.Addresses
{
    public class CityLookup : Lookup<Guid>
    {
        public virtual IReactiveList<BarangayLookup> Barangays { get; set; }
    }
}
