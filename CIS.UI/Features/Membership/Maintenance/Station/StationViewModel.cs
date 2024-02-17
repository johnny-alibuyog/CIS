﻿using System;
using System.Windows.Media.Imaging;
using CIS.Core.Domain.Common;
using CIS.Core.Domain.Membership;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Common.Address;
using CIS.UI.Utilities.Extentions;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Maintenance.Station;

public class StationViewModel : ViewModelBase
{
    private readonly StationController _controller;

    public virtual Guid Id { get; set; }

    public virtual BitmapSource Logo { get; set; }

    [NotNullNotEmpty(Message = "Descrition is mandatory.")]
    public virtual string Name { get; set; }

    [NotNullNotEmpty(Message = "Office is mandatory.")]
    public virtual string Office { get; set; }

    [NotNullNotEmpty(Message = "Location is mandatory.")]
    public virtual string Location { get; set; }

    public virtual decimal? ClearanceFee { get; set; }

    //[IsNumeric(Message = "Clerance validity is numeric.")]
    public virtual int? ClearanceValidityInDays { get; set; }

    [Valid]
    [NotNull(Message = "Location is mandatory.")]
    public virtual AddressViewModel Address { get; set; }

    public virtual IReactiveCommand LookupLogo { get; set; }

    public virtual IReactiveCommand Save { get; set; }

    public virtual IReactiveCommand Refresh { get; set; }

    public StationViewModel()
    {
        this.Address = new AddressViewModel();

        this.WhenAnyValue(x => x.Address.IsValid)
            .Subscribe(_ => this.Revalidate());

        //_controller = new StationController(this);
        _controller = IoC.Container.Resolve<StationController>(new ViewModelDependency(this));
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
            target.Name = source.Name;
            target.Office = source.Office;
            target.Location = source.Location;
            target.ClearanceFee = source.ClearanceFee;
            target.ClearanceValidityInDays = source.ClearanceValidityInDays;
            target.Address.SerializeWith(source.Address);
            //target.Officers = source.Officers;
            return target;
        }
        else if (instance is Core.Domain.Membership.Station)
        {
            var source = instance as Core.Domain.Membership.Station;
            var target = this;

            target.Id = source.Id;
            target.Logo = source.Logo.Image.ToBitmapSource();
            target.Name = source.Name;
            target.Office = source.Office;
            target.Location = source.Location;
            target.ClearanceFee = source.ClearanceFee;
            target.ClearanceValidityInDays = source.ClearanceValidityInDays;
            target.Address.SerializeWith(source.Address);
            //target.Officers = source.Officers
            //    .Select(x => new OfficerViewModel().SerializeWith(x) as OfficerViewModel)
            //    .ToReactiveColletion();

            return target;
        }

        return null;
    }

    public override object DeserializeInto(object instance)
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
        else if (instance is Core.Domain.Membership.Station)
        {
            var source = this;
            var target = instance as Core.Domain.Membership.Station;

            //target.Id = source.Id;
            target.Logo.Image = source.Logo.ToImage();
            target.Name = source.Name;
            target.Office = source.Office;
            target.Location = source.Location;
            target.ClearanceFee = source.ClearanceFee;
            target.ClearanceValidityInDays = source.ClearanceValidityInDays;
            target.Address = (Address)source.Address.DeserializeInto(new Address());
            //target.Officers = source.Officers
            //    .Select(x => x.SerializeInto(new Officer()) as Officer)
            //    .ToReactiveColletion();

            return target;
        }

        return null;
    }
}