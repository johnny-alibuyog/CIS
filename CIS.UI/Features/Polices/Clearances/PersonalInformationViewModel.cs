using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;
using ReactiveUI;

namespace CIS.UI.Features.Polices.Clearances
{
    public class PersonalInformationViewModel : ViewModelBase
    {
        public virtual PersonViewModel Person { get; set; }

        public virtual string Height { get; set; }

        public virtual string Weight { get; set; }

        public virtual string AlsoKnownAs { get; set; }

        public virtual string Purpose { get; set; }

        //public virtual string NewPurpose { get; set; }

        public virtual AddressViewModel Address { get; set; }

        public virtual string BirthPlace { get; set; }

        public virtual string Occupation { get; set; }

        public virtual string Religion { get; set; }

        public virtual ReactiveCollection<string> Purposes { get; set; }

        public virtual LookupBase<Guid> VerifiedBy { get; set; } 

        public virtual ReactiveCollection<LookupBase<Guid>> Verifiers {get;set;}

        public virtual LookupBase<Guid> CertifiedBy { get; set; }

        public virtual ReactiveCollection<LookupBase<Guid>> Certifiers { get; set; }

        public PersonalInformationViewModel()
        {
            this.Person = new PersonViewModel();
            this.Address = new AddressViewModel();
        }
    }
}
