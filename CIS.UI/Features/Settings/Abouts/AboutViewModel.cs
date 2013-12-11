using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;

namespace CIS.UI.Features.Settings.Abouts
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly AboutController _controller;

        public virtual string About { get; set; }

        public AboutViewModel()
        {
            _controller = IoC.Container.Resolve<AboutController>(new ViewModelDependency(this));
        }
    }
}
