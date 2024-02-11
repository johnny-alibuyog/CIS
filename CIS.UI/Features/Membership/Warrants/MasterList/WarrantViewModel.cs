using System;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.Core.Domain.Membership;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Common.Address;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Warrants.MasterList;

public class WarrantViewModel : ViewModelBase
{
    private readonly WarrantController _controller;

    public virtual Guid Id { get; set; }

    public virtual string CaseNumber { get; set; }

    public virtual string Crime { get; set; }

    public virtual string Description { get; set; }

    public virtual string Remarks { get; set; }

    public virtual decimal BailAmount { get; set; }

    public virtual DateTime? IssuedOn { get; set; }

    public virtual string IssuedBy { get; set; }

    public virtual AddressViewModel IssuedAt { get; set; }

    public virtual SuspectViewModel SelectedSuspect { get; set; }

    public virtual IReactiveList<SuspectViewModel> Suspects { get; set; }

    public virtual IReactiveCommand Load { get; set; }

    public virtual IReactiveCommand CreateSupect { get; set; }

    public virtual IReactiveCommand EditSuspect { get; set; }

    public virtual IReactiveCommand DeleteSuspect { get; set; }

    public virtual IReactiveCommand BatchSave { get; set; }

    public WarrantViewModel()
    {
        this.IssuedAt = new AddressViewModel();
        this.Suspects = new ReactiveList<SuspectViewModel>();

        this.WhenAnyValue(x => x.IssuedAt)
            .Subscribe(x => this.Revalidate());

        //_controller = new WarrantController(this);
        _controller = IoC.Container.Resolve<WarrantController>(new ViewModelDependency(this));
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
                .ToReactiveList();

            return target;
        }

        return null;
    }

    public override object DeserializeInto(object instance)
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
            target.IssuedAt = (Address)source.IssuedAt.DeserializeInto(new Address());
            target.Suspects = source.Suspects
                .Select(x => (Suspect)x.DeserializeInto(new Suspect()))
                .ToList();

            return target;
        }

        return null;
    }
}
