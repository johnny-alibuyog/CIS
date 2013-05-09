using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Stations
{
    public class OfficerListViewModel : ViewModelBase
    {
        private readonly OfficerListController _controller;

        public virtual OfficerListCriteriaViewModel Criteria { get; set; }

        public virtual OfficerListItemViewModel SelectedItem { get; set; }

        public virtual IReactiveCollection<OfficerListItemViewModel> Items { get; set; }

        public virtual IReactiveCommand Search { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public OfficerListViewModel() 
        {
            this.Criteria = new OfficerListCriteriaViewModel();
            _controller = new OfficerListController(this);
        }
    }
}
