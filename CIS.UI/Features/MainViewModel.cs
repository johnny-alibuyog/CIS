using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using FirstFloor.ModernUI.Presentation;
using ReactiveUI;

namespace CIS.UI.Features
{
    public class MainViewModel : ViewModelBase
    {
        private readonly MainController _controller;

        public virtual Uri DefaultPage { get; set; }
        public virtual LinkGroupCollection LinkGroups { get; set; }

        public MainViewModel()
        {
            _controller = IoC.Container.Resolve<MainController>(new ViewModelDependency(this));
        }
    }
}
