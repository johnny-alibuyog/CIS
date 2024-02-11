using System;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.UI.Features.Common.Address;
using CIS.UI.Features.Common.Person;
using CIS.UI.Utilities.Extentions;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Registration.Applications;

public class PersonalInformationViewModel : ViewModelBase
{
    [Valid]
    public virtual PersonViewModel Person { get; set; }

    [NotNullNotEmpty(Message = "Height is mandatory.")]
    public virtual string Height { get; set; }

    [NotNullNotEmpty(Message = "Weight is mandatory.")]
    public virtual string Weight { get; set; }

    public virtual string Build { get; set; }

    public virtual string Marks { get; set; }

    public virtual string AlsoKnownAs { get; set; }

    [Valid]
    public virtual AddressViewModel Address { get; set; }

    public virtual string BirthPlace { get; set; }

    public virtual string Occupation { get; set; }

    public virtual string Religion { get; set; }

    [NotNullNotEmpty(Message = "Citizenship is mandatory.")]
    public virtual string Citizenship { get; set; }

    [NotNull(Message = "Civil status is mandatory.")]
    public virtual CivilStatus? CivilStatus { get; set; }

    public virtual IReactiveList<CivilStatus> CivilStatuses { get; set; }

    [NotNull(Message = "Purpose is mandatory.")]
    public virtual Lookup<Guid> Purpose { get; set; }

    public virtual IReactiveList<Lookup<Guid>> Purposes { get; set; }

    [NotNull(Message = "Verifier is mandatory.")]
    public virtual Lookup<Guid> Verifier { get; set; } 

    public virtual IReactiveList<Lookup<Guid>> Verifiers {get;set;}

    [NotNull(Message = "Certifier is mandatory.")]
    public virtual Lookup<Guid> Certifier { get; set; }

    public virtual IReactiveList<Lookup<Guid>> Certifiers { get; set; }

    public PersonalInformationViewModel()
    {
        this.CivilStatuses = Enum.GetValues(typeof(CivilStatus))
            .Cast<CivilStatus>()
            .ToReactiveList();

        this.Citizenship = "Filipino";
        this.CivilStatus = Core.Domain.Common.CivilStatus.Single;

        this.Person = new PersonViewModel();
        this.Address = new AddressViewModel();

        this.WhenAnyValue(
            x => x.Person.IsValid,
            x => x.Address.IsValid,
            (isPersonValid, isAddressValid) => true
        )
        .Subscribe(_ => this.Revalidate());
    }
}
