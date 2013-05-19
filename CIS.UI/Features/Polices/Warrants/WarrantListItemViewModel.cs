using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CIS.UI.Features.Polices.Warrants
{
    public class WarrantListItemViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }
        public virtual Guid SuspectId { get; set; }
        public virtual string Suspect { get; set; }
        public virtual string Crime { get; set; }
        public virtual Nullable<DateTime> IssuedOn { get; set; }
    }
}
