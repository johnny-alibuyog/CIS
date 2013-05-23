using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Maintenances
{
    public class PurposeListViewModel : ViewModelBase
    {
        private PurposeListController _controller;

        public virtual string NewItem { get; set; }

        public virtual PurposeViewModel SelectedItem { get; set; }

        public virtual ReactiveCollection<PurposeViewModel> Items { get; set; }

        public virtual IReactiveCommand Load { get; set; }

        public virtual IReactiveCommand Insert { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public virtual IReactiveCommand Search { get; set; }

        public PurposeListViewModel()
        {
            _controller = new PurposeListController(this);
        }
    }
}
