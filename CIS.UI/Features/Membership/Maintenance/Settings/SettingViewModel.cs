﻿using System;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.Core.Domain.Membership;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Common.Biometrics;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Maintenance.Settings;

public class SettingViewModel : ViewModelBase
{
    private readonly SettingController _controller;

    public virtual Guid Id { get; set; }

    public virtual bool WithCameraDevice { get; set; }

    public virtual bool WithFingerScannerDevice { get; set; }

    public virtual bool WithDigitalSignatureDevice { get; set; }

    public virtual SettingFingerViewModel SelectedItem { get; set; }

    public virtual IReactiveList<SettingFingerViewModel> FingersToScan { get; set; }

    public virtual IReactiveCommand Load { get; set; }

    public virtual IReactiveCommand Save { get; set; }

    public SettingViewModel()
    {
        this.FingersToScan = FingerViewModel.All.Select(x => new SettingFingerViewModel(x)).ToReactiveList();

        //_controller = new SettingController(this);
        _controller = IoC.Container.Resolve<SettingController>(new ViewModelDependency(this));
    }

    public override object SerializeWith(object instance)
    {
        var source = instance as Setting;
        if (source == null)
            return null;

        var target = this;
        target.Id = source.Id;
        target.WithCameraDevice = source.WithCameraDevice;
        target.WithFingerScannerDevice = source.WithFingerScannerDevice;
        target.WithDigitalSignatureDevice = source.WithDigitalSignatureDevice;

        foreach (var item in target.FingersToScan)
        {
            item.Include = source.FingersToScan.Any(x => x.Id == item.Finger.Id);
        }

        return target;
    }

    public override object DeserializeInto(object instance)
    {
        var target = instance as Setting;
        if (target == null)
            return null;

        var source = this;

        //target.Id = source.Id;
        target.WithCameraDevice = source.WithCameraDevice;
        target.WithFingerScannerDevice = source.WithFingerScannerDevice;
        target.WithDigitalSignatureDevice = source.WithDigitalSignatureDevice;
        target.FingersToScan = Finger.All
            .Where(x => source.FingersToScan
                .Any(o =>
                    o.Include == true &&
                    o.Finger.Id == x.Id 
                )
            )
            .AsEnumerable();                    

        return target;
    }
}