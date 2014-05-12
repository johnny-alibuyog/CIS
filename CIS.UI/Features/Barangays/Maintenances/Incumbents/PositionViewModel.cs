using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Maintenances.Incumbents
{
    public class PositionViewModel : Lookup<string>
    {
        public virtual IReactiveList<Lookup<string>> Committees { get; set; }

        public PositionViewModel() { }

        public PositionViewModel(string id, string name) : base(id, name) { }
    }
}
