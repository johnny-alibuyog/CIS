using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CIS.Core.Entities.Barangays;
using CIS.Core.Entities.Commons;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Utilities.Extentions;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Maintenances.Offices
{
    public class OfficeViewModel: ViewModelBase
    {
        private readonly OfficeController _controller;

        public virtual Guid Id { get; set; }

        public virtual BitmapSource Logo { get; set; }

        [NotNullNotEmpty(Message = "Name is mandatory.")]
        public virtual string Name { get; set; }

        [NotNullNotEmpty(Message = "Location is mandatory.")]
        public virtual string Location { get; set; }

        [Valid]
        [NotNull(Message = "Location is mandatory.")]
        public virtual AddressViewModel Address { get; set; }

        public virtual Nullable<decimal> ClearanceFee { get; set; }

        public virtual Nullable<decimal> CertificationFee { get; set; }

        public virtual Nullable<decimal> DocumentStampTax { get; set; }

        public virtual IReactiveCommand LookupLogo { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public virtual IReactiveCommand Refresh { get; set; }

        public OfficeViewModel()
        {
            this.Address = new AddressViewModel();

            this.WhenAnyValue(x => x.Address.IsValid)
                .Subscribe(_ => this.Revalidate());

            _controller = IoC.Container.Resolve<OfficeController>(new ViewModelDependency(this));
        }

        public override object SerializeWith(object instance)
        {
            if (instance == null)
                return null;

            if (instance is OfficeViewModel)
            {
                var source = instance as OfficeViewModel;
                var target = this;

                target.Id = source.Id;
                target.Logo = source.Logo;
                target.Name = source.Name;
                target.Location = source.Location;
                target.Address.SerializeWith(source.Address);
                target.ClearanceFee = source.ClearanceFee;
                target.CertificationFee = source.CertificationFee;
                target.DocumentStampTax = source.DocumentStampTax;

                return target;
            }
            else if (instance is Office)
            {
                var source = instance as Office;
                var target = this;

                target.Id = source.Id;
                target.Logo = source.Logo.Image.ToBitmapSource();
                target.Name = source.Name;
                target.Location = source.Location;
                target.Address.SerializeWith(source.Address);
                target.ClearanceFee = source.ClearanceFee;
                target.CertificationFee = source.CertificationFee;
                target.DocumentStampTax = source.DocumentStampTax;

                return target;
            }

            return null;
        }

        public override object DeserializeInto(object instance)
        {
            if (instance == null)
                return null;

            if (instance is OfficeViewModel)
            {
                var source = this;
                var target = instance as OfficeViewModel;

                source.SerializeWith(target);
                return target;
            }
            else if (instance is Office)
            {
                var source = this;
                var target = instance as Office;

                //target.Id = source.Id;
                target.Logo.Image = source.Logo.ToImage();
                target.Name = source.Name;
                target.Location = source.Location;
                target.Address = (Address)source.Address.DeserializeInto(new Address());
                target.ClearanceFee = source.ClearanceFee;
                target.CertificationFee = source.CertificationFee;
                target.DocumentStampTax = source.DocumentStampTax;

                return target;
            }

            return null;
        }
    }
}
