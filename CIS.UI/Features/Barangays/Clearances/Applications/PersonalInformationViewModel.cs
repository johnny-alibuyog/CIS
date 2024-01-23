using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using System;

namespace CIS.UI.Features.Barangays.Clearances.Applications;

public class PersonalInformationViewModel : ViewModelBase
{
    [Valid]
    public virtual PersonViewModel Person { get; set; }

    [Valid]
    public virtual AddressViewModel Address { get; set; }

    [NotNull(Message = "Purpose is mandatory.")]
    public virtual Lookup<Guid> Purpose { get; set; }

    public virtual IReactiveList<Lookup<Guid>> Purposes { get; set; }

    public PersonalInformationViewModel()
    {
        this.Person = new PersonViewModel();
        this.Address = new AddressViewModel();

        this.WhenAnyValue(
            x => x.Person.IsValid,
            x => x.Address.IsValid,
            (
                isPersonValid, 
                isAddressValid
            ) => true
        )
        .Subscribe(_ => this.Revalidate());
    }
}
