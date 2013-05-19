using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class LicenseListItemViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual string Owner { get; set; }

        public virtual string Gun { get; set; }

        public virtual DateTime ExpiryDate { get; set; }
    }
}
