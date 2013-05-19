using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Maintenances
{
    public class KindListViewModel : ViewModelBase
    {
        private KindListController _controller;

        public virtual string NewItem { get; set; }

        public virtual KindViewModel SelectedItem { get; set; }

        public virtual ReactiveCollection<KindViewModel> Items { get; set; }

        public virtual IReactiveCommand Load { get; set; }

        public virtual IReactiveCommand Insert { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public virtual IReactiveCommand Search { get; set; }

        public KindListViewModel()
        {
            _controller = new KindListController(this);
        }
    }
}
