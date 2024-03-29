﻿using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Maintenances.Purposes;

public class PurposeListViewModel : ViewModelBase
{
    private PurposeListController _controller;

    public virtual string NewItem { get; set; }

    public virtual PurposeViewModel SelectedItem { get; set; }

    public virtual IReactiveList<PurposeViewModel> Items { get; set; }

    public virtual IReactiveCommand Load { get; set; }

    public virtual IReactiveCommand Insert { get; set; }

    public virtual IReactiveCommand Delete { get; set; }

    public virtual IReactiveCommand Search { get; set; }

    public PurposeListViewModel()
    {
        //_controller = new PurposeListController(this);
        _controller = IoC.Container.Resolve<PurposeListController>(new ViewModelDependency(this));
    }
}
