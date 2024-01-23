using CIS.UI.Utilities.Extentions;
using ReactiveUI;
using System;

namespace CIS.UI.Features.Commons.Signatures;

public class SignatureDialogController : ControllerBase<SignatureDialogViewModel>
{
    public SignatureDialogController(SignatureDialogViewModel viewModel) : base(viewModel)
    {
        this.ViewModel.Accept = new ReactiveCommand();
        this.ViewModel.Accept.Subscribe(x => this.ViewModel.Close());
        this.ViewModel.Accept.ThrownExceptions.Handle(this);
    }
}
