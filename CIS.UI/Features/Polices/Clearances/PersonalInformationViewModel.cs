using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Polices.Clearances
{
    public class PersonalInformationViewModel : ViewModelBase
    {
        [Valid]
        public virtual PersonViewModel Person { get; set; }

        public virtual string Height { get; set; }

        public virtual string Weight { get; set; }

        public virtual string AlsoKnownAs { get; set; }

        [Valid]
        public virtual AddressViewModel Address { get; set; }

        public virtual string BirthPlace { get; set; }

        public virtual string Occupation { get; set; }

        public virtual string Religion { get; set; }

        public virtual Lookup<Guid> Purpose { get; set; }

        public virtual ReactiveCollection<Lookup<Guid>> Purposes { get; set; }

        public virtual Lookup<Guid> Verifier { get; set; } 

        public virtual ReactiveCollection<Lookup<Guid>> Verifiers {get;set;}

        public virtual Lookup<Guid> Certifier { get; set; }

        public virtual ReactiveCollection<Lookup<Guid>> Certifiers { get; set; }

        public PersonalInformationViewModel()
        {
            this.Person = new PersonViewModel();
            this.Address = new AddressViewModel();

            this.WhenAny(
                x => x.Person.IsValid,
                x => x.Address.IsValid,
                (isPersonValid, isAddressValid) => { return true; }
            )
            .Subscribe(_ => this.Revalidate());
        }
    }
}
