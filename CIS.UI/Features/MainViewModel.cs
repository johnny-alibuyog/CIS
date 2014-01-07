using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;

namespace CIS.UI.Features
{
    public class MainViewModel : ViewModelBase
    {
        private readonly MainController _controller;

        public MainViewModel()
        {
            _controller = IoC.Container.Resolve<MainController>(new ViewModelDependency(this));
        }
    }
}
