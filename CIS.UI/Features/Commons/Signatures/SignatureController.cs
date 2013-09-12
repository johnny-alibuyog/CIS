using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;

namespace CIS.UI.Features.Commons.Signatures
{
    public class SignatureController : ControllerBase<SignatureViewModel>
    {
        public SignatureController(SignatureViewModel viewModel) : base(viewModel) { }

        [HandleError]
        public virtual void Capture() { }

        [HandleError]
        public virtual void Clear()
        {
            //this.ViewModel.Strokes.Clear();
            this.ViewModel.SignatureImage = null;
        }
    }
}
