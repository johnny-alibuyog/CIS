using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Barangays.Blotters.MasterList
{
    public class BlotterOfficialViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual bool Selected { get; set; }

        public virtual string Name { get; set; }

        public virtual string Position { get; set; }
    }
}
