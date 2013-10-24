using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Features.Polices.Maintenances;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features
{
    public class Window1Controller : ControllerBase<Window1ViewModel>
    {
        public Window1Controller(Window1ViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.Submit = new ReactiveCommand(this.ViewModel.IsValidObservable());
            this.ViewModel.Submit.Subscribe(x =>
            {
                var dialog = new DialogService<OfficerView, OfficerViewModel>();
                dialog.ViewModel.Ranks = new ReactiveList<Lookup<string>>() 
                { 
                    new Lookup<string>("1", "value1"),  
                    new Lookup<string>("2", "value2"),  
                    new Lookup<string>("3", "value3"),  
                };

                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(o => this.MessageBox.Confirm("yeah"));
                dialog.ViewModel.Save.ThrownExceptions.Handle(this);

                dialog.ShowModal();
            });
        }
    }
}
