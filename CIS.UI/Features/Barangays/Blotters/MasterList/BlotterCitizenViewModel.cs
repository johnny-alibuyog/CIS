using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;

namespace CIS.UI.Features.Barangays.Blotters.MasterList
{
    public class BlotterCitizenViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual Nullable<Gender> Gender { get; set; }
    }
}
