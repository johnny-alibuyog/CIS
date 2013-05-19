using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class LicenseViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual PersonViewModel Person { get; set; }

        public virtual AddressViewModel Address { get; set; }

        public virtual GunViewModel Gun { get; set; }

        public virtual string LicenseNumber { get; set; }

        public virtual string ControlNumber { get; set; }

        public virtual DateTime IssueDate { get; set; }

        public virtual DateTime ExpirationDate { get; set; }

        public virtual IReactiveCommand Load { get; set; }

        public virtual IReactiveCommand Save { get; set; }
    }
}
