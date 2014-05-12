using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Commons.Biometrics
{
    public class FingerScannerDialogController : ControllerBase<FingerScannerDialogViewModel>
    {
        public FingerScannerDialogController(FingerScannerDialogViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Accept = new ReactiveCommand();
            this.ViewModel.Accept.Subscribe(x => this.ViewModel.Close());
            this.ViewModel.Accept.ThrownExceptions.Handle(this);
        }
    }
}
