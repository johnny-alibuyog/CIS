using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Warrants
{
    public class SuspectViewModel : ViewModelBase
    {
        private readonly SuspectController _controller;

        public virtual Guid Id { get; set; }

        public virtual Nullable<ArrestStatus> ArrestStatus { get; set; }

        public virtual PersonViewModel Person { get; set; }

        public virtual PhysicalAttributesViewModel PhysicalAttributes { get; set; }

        public virtual AddressViewModel Address { get; set; }

        public virtual ReactiveList<string> Aliases { get; set; }

        public virtual ReactiveList<string> Occupations { get; set; }

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

            _controller = new SuspectController(this);
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

        public override object SerializeInto(object instance)
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

                target.Id = source.Id;
                target.ArrestStatus = source.ArrestStatus;
                target.Person = (Person)source.Person.SerializeInto(new Person());
                target.PhysicalAttributes = (PhysicalAttributes)source.PhysicalAttributes.SerializeInto(new PhysicalAttributes());
                target.Address = (Address)source.Address.SerializeInto(new Address());
                target.Aliases = source.Aliases;
                target.Occupations = source.Occupations;

                return target;
            }

            return null;
        }
    }
}
