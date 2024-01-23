using CIS.UI.Utilities.Extentions;
using ReactiveUI;
using System;

namespace CIS.UI.Features.Barangays.Citizens;

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
