using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Utilities.Extentions;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Maintenances
{
    public class StationViewModel : ViewModelBase
    {
        private readonly StationController _controller;

        public virtual Guid Id { get; set; }

        public virtual BitmapSource Logo { get; set; }

        [NotNullNotEmpty(Message = "Office is mandatory.")]
        public virtual string Office { get; set; }

        [NotNullNotEmpty(Message = "Location is mandatory.")]
        public virtual string Location { get; set; }

        //[IsNumeric(Message = "Clerance validity is numeric.")]
        public virtual int ClearanceValidityInDays { get; set; }

        [NotNull(Message = "Location is mandatory.")]
        public virtual AddressViewModel Address { get; set; }

        public virtual IReactiveCommand LookupLogo { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public virtual IReactiveCommand Refresh { get; set; }

        public StationViewModel()
        {
            this.Address = new AddressViewModel();
            _controller = new StationController(this);
        }

        public override object SerializeWith(object instance)
        {
            if (instance == null)
                return null;

            if (instance is StationViewModel)
            {
                var source = instance as StationViewModel;
                var target = this;

                target.Id = source.Id;
                target.Logo = source.Logo;
                target.Office = source.Office;
                target.Location = source.Location;
                target.ClearanceValidityInDays = source.ClearanceValidityInDays;
                target.Address.SerializeWith(source.Address);
                //target.Officers = source.Officers;
                return target;
            }
            else if (instance is Station)
            {
                var source = instance as Station;
                var target = this;

                target.Id = source.Id;
                target.Logo = source.Logo.Image.ToBitmapSource();
                target.Office = source.Office;
                target.Location = source.Location;
                target.ClearanceValidityInDays = source.ClearanceValidityInDays;
                target.Address.SerializeWith(source.Address);
                //target.Officers = source.Officers
                //    .Select(x => new OfficerViewModel().SerializeWith(x) as OfficerViewModel)
                //    .ToReactiveColletion();

                return target;
            }

            return null;
        }

        public override object SerializeInto(object instance)
        {
            if (instance == null)
                return null;

            if (instance is StationViewModel)
            {
                var source = this;
                var target = instance as StationViewModel;

                source.SerializeWith(target);
                return target;
            }
            else if (instance is Station)
            {
                var source = this;
                var target = instance as Station;

                //target.Id = source.Id;
                target.Logo.Image = source.Logo.ToImage();
                target.Office = source.Office;
                target.Location = source.Location;
                target.ClearanceValidityInDays = source.ClearanceValidityInDays;
                target.Address = (Address)source.Address.SerializeInto(new Address());
                //target.Officers = source.Officers
                //    .Select(x => x.SerializeInto(new Officer()) as Officer)
                //    .ToReactiveColletion();

                return target;
            }

            return null;
        }
    }
}
