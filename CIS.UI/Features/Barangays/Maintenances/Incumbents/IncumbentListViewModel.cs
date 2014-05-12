using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Maintenances.Incumbents
{
    public class IncumbentListViewModel : ViewModelBase
    {
        private readonly IncumbentListController _controller;

        public virtual IReactiveList<IncumbentListItemViewModel> Items { get; set; }

        public virtual IncumbentListItemViewModel SelectedItem { get; set; }

        public virtual IReactiveCommand Refresh { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public IncumbentListViewModel()
        {
            //_controller = new IncumbentListController(this);
            _controller = IoC.Container.Resolve<IncumbentListController>(new ViewModelDependency(this));
        }
    }
}
