using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Polices.Stations
{
    public class OfficerListItemViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Rank { get; set; }
    }
}
