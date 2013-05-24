using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Commons.Signatures
{
    public class SignatureDialogController : ControllerBase<SignatureDialogViewModel>
    {
        public SignatureDialogController(SignatureDialogViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.Accept = new ReactiveCommand();
            this.ViewModel.Accept.Subscribe(x => this.ViewModel.ActionResult = true);
        }
    }
}
