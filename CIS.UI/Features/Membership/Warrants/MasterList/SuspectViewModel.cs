using CIS.Core.Domain.Common;
using CIS.Core.Domain.Membership;
using CIS.UI.Features.Common.Address;
using CIS.UI.Features.Common.Person;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using System;

namespace CIS.UI.Features.Membership.Warrants.MasterList;

public class SuspectViewModel : ViewModelBase
{
    private readonly SuspectController _controller;

    public virtual Guid Id { get; set; }

    public virtual ArrestStatus? ArrestStatus { get; set; }

    [Valid]
    public virtual PersonViewModel Person { get; set; }

    public virtual PhysicalAttributesViewModel PhysicalAttributes { get; set; }

    [Valid]
    public virtual AddressViewModel Address { get; set; }

    public virtual IReactiveList<string> Aliases { get; set; }

    public virtual IReactiveList<string> Occupations { get; set; }

    public virtual string AliasToAdd { get; set; }

    public virtual string OccupationToAdd { get; set; }

    public virtual IReactiveCommand AddAlias { get; set; }

    public virtual IReactiveCommand DeleteAlias { get; set; }

    public virtual IReactiveCommand AddOccupation { get; set; }

    public virtual IReactiveCommand DeleteOccupation { get; set; }

    public virtual IReactiveCommand Save { get; set; }

    public SuspectViewModel()
    {
        this.Person = new PersonViewModel();
        this.PhysicalAttributes = new PhysicalAttributesViewModel();
        this.Address = new AddressViewModel();
        this.Aliases = new ReactiveList<string>();
        this.Occupations = new ReactiveList<string>();

        this.WhenAnyValue(
            x => x.Person.IsValid,
            x => x.Address.IsValid,
            (isPersonValid, isAddressValid) => true
        )
        .Subscribe(x => this.Revalidate());

        _controller = new SuspectController(this);
        //_controller = IoC.Container.Resolve<SuspectController>(new ViewModelDependency(this));
    }

    public override object SerializeWith(object instance)
    {
        if (instance == null)
            return null;

        if (instance is SuspectViewModel)
        {
            var source = instance as SuspectViewModel;
            var target = this;

            target.Id = source.Id;
            target.ArrestStatus = source.ArrestStatus;
            target.Person.SerializeWith(source.Person);
            target.PhysicalAttributes.SerializeWith(source.PhysicalAttributes);
            target.Address.SerializeWith(source.Address);
            target.Aliases = source.Aliases;
            target.Occupations = source.Occupations;
            return target;
        }
        else if (instance is Suspect)
        {
            var source = instance as Suspect;
            var target = this;

            target.Id = source.Id;
            target.ArrestStatus = source.ArrestStatus;
            target.Person.SerializeWith(source.Person);
            target.PhysicalAttributes.SerializeWith(source.PhysicalAttributes);
            target.Address.SerializeWith(source.Address);
            target.Aliases = new ReactiveList<string>(source.Aliases);
            target.Occupations = new ReactiveList<string>(source.Occupations);
            return target;
        }

        return null;
    }

    public override object DeserializeInto(object instance)
    {
        if (instance == null)
            return null;

        if (instance is SuspectViewModel)
        {
            var source = this;
            var target = instance as SuspectViewModel;

            source.SerializeWith(target);
            return target;
        }
        else if (instance is Suspect)
        {
            var source = this;
            var target = instance as Suspect;

            target.WithId(source.Id);
            target.ArrestStatus = source.ArrestStatus;
            target.Person = (Person)source.Person.DeserializeInto(new Person());
            target.PhysicalAttributes = (PhysicalAttribute)source.PhysicalAttributes.DeserializeInto(new PhysicalAttribute());
            target.Address = (Address)source.Address.DeserializeInto(new Address());
            target.Aliases = source.Aliases;
            target.Occupations = source.Occupations;

            return target;
        }

        return null;
    }
}
