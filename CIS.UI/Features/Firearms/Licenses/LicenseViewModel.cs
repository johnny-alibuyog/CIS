using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class LicenseViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual PersonViewModel Person { get; set; }

        public virtual AddressViewModel Address { get; set; }



    }
}
