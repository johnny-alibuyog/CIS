using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;

namespace CIS.UI.Features.Commons.Addresses
{
    public class AddressViewModel : ViewModelBase
    {
        private readonly AddressController _controller;

        public virtual string Address1 { get; set; }

        public virtual string Address2 { get; set; }

        public virtual string Barangay { get; set; }

        public virtual string City { get; set; }

        public virtual string Province { get; set; }

        public virtual Lookup<Guid> SelectedBarangay { get; set; }

        public virtual Lookup<Guid> SelectedCity { get; set; }

        public virtual Lookup<Guid> SelectedProvince { get; set; }

        public virtual IReactiveCollection<Lookup<Guid>> Barangays { get; set; }

        public virtual IReactiveCollection<Lookup<Guid>> Cities { get; set; }

        public virtual IReactiveCollection<Lookup<Guid>> Provinces { get; set; }

        public AddressViewModel()
        {
            //_controller = new AddressController(this);
            _controller = IoC.Container.Resolve<AddressController>(new ViewModelDependency(this));
        }

        public override object SerializeWith(object instance)
        {
            if (instance == null)
                return null;

            if (instance is AddressViewModel)
            {
                var source = instance as AddressViewModel;
                var target = this;

                target.Address1 = source.Address1;
                target.Address2 = source.Address2;
                target.Barangay = source.Barangay;
                target.City = source.City;
                target.Province = source.Province;
                return target;
            }
            else if (instance is Address)
            {
                var source = instance as Address;
                var target = this;

                target.Address1 = source.Address1;
                target.Address2 = source.Address2;
                target.Barangay = source.Barangay;
                target.City = source.City;
                target.Province = source.Province;
                return target;
            }

            return null;
        }

        public override object SerializeInto(object instance)
        {
            if (instance == null)
                return null;

            if (instance is AddressViewModel)
            {
                var source = this;
                var target = instance as AddressViewModel;

                target.SerializeWith(source);
                return target;
            }
            else if (instance is Address)
            {
                var source = this;
                var target = instance as Address;

                target.Address1 = source.Address1;
                target.Address2 = source.Address2;
                target.Barangay = source.Barangay;
                target.City = source.City;
                target.Province = source.Province;

                return target;
            }

            return null;
        }

        public override string ToString()
        {
            return Address1 + " " + Address2 + " " + Barangay + " " + City + " " + Province;
        }
    }
}
