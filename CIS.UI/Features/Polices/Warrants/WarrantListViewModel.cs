using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Warrants
{
    public class WarrantListViewModel : ViewModelBase
    {
        private readonly WarrantListController _controller;

        public virtual WarrantListCriteriaViewModel Criteria { get; set; }

        public virtual WarrantListItemViewModel SelectedItem { get; set; }

        public virtual IReactiveCollection<WarrantListItemViewModel> Items { get; set; }

        public virtual IReactiveCommand Search { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public WarrantListViewModel() 
        {
            this.Criteria = new WarrantListCriteriaViewModel();
            //_controller = new WarrantListController(this);
            _controller = IoC.Container.Resolve<WarrantListController>(new ViewModelDependency(this));
        }
    }
}
