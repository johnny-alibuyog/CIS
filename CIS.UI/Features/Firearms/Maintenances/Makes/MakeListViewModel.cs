using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Maintenances.Makes
{
    public class MakeListViewModel: ViewModelBase
    {
        private MakeListController _controller;

        public virtual string NewItem { get; set; }

        public virtual MakeViewModel SelectedItem { get; set; }

        public virtual IReactiveList<MakeViewModel> Items { get; set; }

        public virtual IReactiveCommand Load { get; set; }

        public virtual IReactiveCommand Insert { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public virtual IReactiveCommand Search { get; set; }

        public MakeListViewModel()
        {
            //_controller = new MakeListController(this);
            _controller = IoC.Container.Resolve<MakeListController>(new ViewModelDependency(this));
        }
    }
}
