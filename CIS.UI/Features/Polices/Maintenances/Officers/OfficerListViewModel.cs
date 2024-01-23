using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;

namespace CIS.UI.Features.Polices.Maintenances.Officers;

public class OfficerListViewModel : ViewModelBase
{
    private readonly OfficerListController _controller;

    public virtual OfficerListCriteriaViewModel Criteria { get; set; }

    public virtual OfficerListItemViewModel SelectedItem { get; set; }

    public virtual IReactiveList<OfficerListItemViewModel> Items { get; set; }

    public virtual IReactiveCommand Search { get; set; }

    public virtual IReactiveCommand Create { get; set; }

    public virtual IReactiveCommand Edit { get; set; }

    public virtual IReactiveCommand Delete { get; set; }

    public OfficerListViewModel() 
    {
        //_controller = new OfficerListController(this);
        _controller = IoC.Container.Resolve<OfficerListController>(new ViewModelDependency(this));
    }
}
