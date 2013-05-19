using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class GunViewModel : ViewModelBase
    {
        public virtual string Model { get; set; }

        public virtual string Caliber { get; set; }

        public virtual string SerialNumber { get; set; }

        //public virtual Lookup<Guid> 
    }
}
