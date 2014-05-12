using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using CIS.UI.Utilities.Extentions;

namespace CIS.UI.Features.Barangays.Citizens
{
    public class CitizenDialogController : ControllerBase<CitizenDialogViewModel>
    {
        public CitizenDialogController(CitizenDialogViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Save = new ReactiveCommand(this.ViewModel.Citizen.IsValidObservable());
            this.ViewModel.Save.Subscribe(x => Save());
            this.ViewModel.Save.ThrownExceptions.Handle(this);
        }

        public virtual void Save()
        {
            this.ViewModel.Citizen.Save.Execute(null);
            this.ViewModel.Close();
        }
    }
}
