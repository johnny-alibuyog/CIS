using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Polices.Warrants;

namespace CIS.UI.Features
{
    public class MainController : ControllerBase<MainViewModel>
    {
        private readonly IEnumerable<ITaskScheduler> _tasks;

        public MainController(MainViewModel viewModel) : base(viewModel)
        {
            if (App.Config.DataStore.Syncronize)
            {
                _tasks = IoC.Container.ResolveAll<ITaskScheduler>();
                foreach (var task in _tasks)
                {
                    task.StartWork();
                }
            }
        }
    }
}
