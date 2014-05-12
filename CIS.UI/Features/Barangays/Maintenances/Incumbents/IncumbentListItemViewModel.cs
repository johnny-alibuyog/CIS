using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Maintenances.Incumbents
{
    public class IncumbentListItemViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual int Year { get; set; }

        public virtual string Term { get; set; }

        public virtual string Captain { get; set; }
    }
}
