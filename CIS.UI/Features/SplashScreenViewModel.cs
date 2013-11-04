using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;

namespace CIS.UI.Features
{
    public class SplashScreenViewModel : ViewModelBase
    {
        private readonly SplashScreenController _controller;

        public virtual string Licensee { get; set; }

        public virtual IEnumerable<string> Plugins { get; set; }

        public virtual string Message { get; set; } 

        public SplashScreenViewModel()
        {
            _controller = IoC.Container.Resolve<SplashScreenController>(new ViewModelDependency(this));
        } 

    }
}
