using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Firearms;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class LicenseViewModel : ViewModelBase
    {
        private readonly LicenseContoller _controller;

        public virtual Guid Id { get; set; }

        [NotNull]
        public virtual PersonViewModel Person { get; set; }

        [NotNull]
        public virtual AddressViewModel Address { get; set; }

        [Valid]
        public virtual GunViewModel Gun { get; set; }

        [NotNullNotEmpty]
        public virtual string LicenseNumber { get; set; }

        [NotNullNotEmpty]
        public virtual string ControlNumber { get; set; }

        public virtual DateTime IssueDate { get; set; }

        public virtual DateTime ExpiryDate { get; set; }

        public virtual IReactiveCommand Load { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public LicenseViewModel()
        {
            IssueDate = DateTime.Today;
            ExpiryDate = DateTime.Today;

            Person = new PersonViewModel();
            Address = new AddressViewModel();
            Gun = new GunViewModel();

            this.WhenAny(
                x => x.Person.IsValid,
                x => x.Address.IsValid,
                x => x.Gun.IsValid,
                (isPersonValid, isAddressValid, isGunValid) => true
            )
            .Subscribe(_ => this.Revalidate());



            _controller = new LicenseContoller(this);
        }

        public override object SerializeWith(object instance)
        {
            if (instance == null)
                return null;

            if (instance is LicenseViewModel)
            {
                var source = instance as LicenseViewModel;
                var target = this;

                target.Id = source.Id;
                target.Person.SerializeWith(source.Person);
                target.Address.SerializeWith(source.Address);
                target.Gun.SerializeWith(source.Gun);
                target.LicenseNumber = source.LicenseNumber;
                target.ControlNumber = source.ControlNumber;
                target.IssueDate = source.IssueDate;
                target.ExpiryDate = source.ExpiryDate;

                return target;
            }
            else if (instance is License)
            {
                var source = instance as License;
                var target = this;

                target.Id = source.Id;
                target.Person.SerializeWith(source.Person);
                target.Address.SerializeWith(source.Address);
                target.Gun.SerializeWith(source.Gun);
                target.LicenseNumber = source.LicenseNumber;
                target.ControlNumber = source.ControlNumber;
                target.IssueDate = source.IssueDate;
                target.ExpiryDate = source.ExpiryDate;

                return target;
            }

            return null;
        }

        public override object SerializeInto(object instance)
        {
            if (instance == null)
                return null;

            if (instance is LicenseViewModel)
            {
                var source = this;
                var destination = instance as LicenseViewModel;

                destination.SerializeWith(source);
                return destination;
            }
            else if (instance is License)
            {
                var source = this;
                var target = instance as License;

                target.Id = source.Id;
                target.Person = (Person)source.Person.SerializeInto(new Person());
                target.Address = (Address)source.Address.SerializeInto(new Address());
                target.Gun = (Gun)source.Gun.SerializeInto(new Gun());
                target.LicenseNumber = source.LicenseNumber;
                target.ControlNumber = source.ControlNumber;
                target.IssueDate = source.IssueDate;
                target.ExpiryDate = source.ExpiryDate;

                return target;
            }

            return null;
        }
    }
}
