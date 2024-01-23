﻿using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;

namespace CIS.UI.Features.Firearms.Licenses.Registrations;

public class LicenseListViewModel : ViewModelBase
{
    private readonly LicenseListController _controller;

    public virtual LicenseListCriteriaViewModel Criteria { get; set; }

    public virtual LicenseListItemViewModel SelectedItem { get; set; }

    public virtual IReactiveList<LicenseListItemViewModel> Items { get; set; }

    public virtual IReactiveCommand Search { get; set; }

    public virtual IReactiveCommand Create { get; set; }

    public virtual IReactiveCommand Edit { get; set; }

    public virtual IReactiveCommand Delete { get; set; }

    public LicenseListViewModel()
    {
        //_controller = new LicenseListController(this);
        _controller = IoC.Container.Resolve<LicenseListController>(new ViewModelDependency(this));
    }
}
