using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Warrants
{
    public class ListViewModel : ViewModelBase
    {
        private readonly ListController _controller;

        public virtual ListCriteriaViewModel Criteria { get; set; }

        public virtual ListItemViewModel SelectedItem { get; set; }

        public virtual IReactiveCollection<ListItemViewModel> Items { get; set; }

        public virtual IReactiveCommand Search { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public ListViewModel() 
        {
            this.Criteria = new ListCriteriaViewModel();
            _controller = new ListController(this);

        }
    }
}
