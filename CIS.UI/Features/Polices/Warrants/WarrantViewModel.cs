using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Warrants
{
    public class WarrantViewModel : ViewModelBase
    {
        private readonly WarrantController _controller;

        public virtual Guid Id { get; set; }

        public virtual string CaseNumber { get; set; }

        public virtual string Crime { get; set; }

        public virtual string Description { get; set; }

        public virtual string Remarks { get; set; }

        public virtual decimal BailAmount { get; set; }

        public virtual Nullable<DateTime> IssuedOn { get; set; }

        public virtual string IssuedBy { get; set; }

        public virtual AddressViewModel IssuedAt { get; set; }

        public virtual SuspectViewModel SelectedSuspect { get; set; }

        public virtual ReactiveCollection<SuspectViewModel> Suspects { get; set; }

        public virtual IReactiveCommand CreateSupect { get; set; }

        public virtual IReactiveCommand EditSuspect { get; set; }

        public virtual IReactiveCommand DeleteSuspect { get; set; }

        public virtual IReactiveCommand BatchSave { get; set; }

        public WarrantViewModel()
        {
            this.IssuedAt = new AddressViewModel();
            this.Suspects = new ReactiveCollection<SuspectViewModel>();

            _controller = new WarrantController(this);
        }

        public virtual void Populate(Guid id)
        {
            _controller.Populate(id);
        }

        public override object SerializeWith(object instance)
        {
            if (instance == null)
                return null;

            if (instance is WarrantViewModel)
            {
                var source = instance as WarrantViewModel;
                var target = this;

                target.Id = source.Id;
                target.CaseNumber = source.CaseNumber;
                target.Crime = source.Crime;
                target.Description = source.Description;
                target.Remarks = source.Remarks;
                target.BailAmount = source.BailAmount;
                target.IssuedOn = source.IssuedOn;
                target.IssuedBy = source.IssuedBy;
                target.IssuedAt.SerializeWith(source.IssuedAt);
                target.Suspects = source.Suspects;

                return target;
            }
            else if (instance is Warrant)
            {
                var source = instance as Warrant;
                var target = this;

                target.Id = source.Id;
                target.CaseNumber = source.CaseNumber;
                target.Crime = source.Crime;
                target.Description = source.Description;
                target.Remarks = source.Remarks;
                target.BailAmount = source.BailAmount;
                target.IssuedOn = source.IssuedOn;
                target.IssuedBy = source.IssuedBy;
                target.IssuedAt.SerializeWith(source.IssuedAt);
                target.Suspects = source.Suspects
                    .Select(x => new SuspectViewModel().SerializeWith(x) as SuspectViewModel)
                    .ToReactiveColletion();

                return target;
            }

            return null;
        }

        public override object SerializeInto(object instance)
        {
            if (instance == null)
                return null;

            if (instance is WarrantViewModel)
            {
                var source = this;
                var destination = instance as WarrantViewModel;

                destination.SerializeWith(source);
                return destination;
            }
            else if (instance is Warrant)
            {
                var source = this;
                var target = instance as Warrant;

                target.CaseNumber = source.CaseNumber;
                target.Crime = source.Crime;
                target.Description = source.Description;
                target.Remarks = source.Remarks;
                target.BailAmount = source.BailAmount;
                target.IssuedOn = source.IssuedOn;
                target.IssuedBy = source.IssuedBy;
                target.IssuedAt = (Address)source.IssuedAt.SerializeInto(new Address());
                target.Suspects = source.Suspects
                    .Select(x => (Suspect)x.SerializeInto(new Suspect()))
                    .ToList();

                return target;
            }

            return null;
        }
    }
}
