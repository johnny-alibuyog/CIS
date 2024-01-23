using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;
using System;

namespace CIS.UI.Features.Barangays.Blotters.MasterList;

public class BlotterListViewModel : ViewModelBase
{
    private readonly BlotterListController _controller;

    public virtual BlotterListCriteriaViewModel Criteria { get; set; }

    public virtual BlotterListItemViewModel SelectedItem { get; set; }

    public virtual IReactiveList<BlotterListItemViewModel> Items { get; set; }

    public virtual IReactiveCommand Search { get; set; }

    public virtual IReactiveCommand Create { get; set; }

    public virtual IReactiveCommand Edit { get; set; }

    public virtual IReactiveCommand Delete { get; set; }

    public BlotterListViewModel()
    {
        this.Criteria = new BlotterListCriteriaViewModel();
        this.Items = new ReactiveList<BlotterListItemViewModel>();

        this.WhenAnyValue(x => x.Criteria.SearchPersonBy)
            .Subscribe(x => this.Items.Clear());

        _controller = IoC.Container.Resolve<BlotterListController>(new ViewModelDependency(this));
    }
}
