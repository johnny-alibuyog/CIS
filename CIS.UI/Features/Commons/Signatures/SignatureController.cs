using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;

namespace CIS.UI.Features.Commons.Signatures;

[HandleError]
public class SignatureController(SignatureViewModel viewModel) : ControllerBase<SignatureViewModel>(viewModel)
{
    public virtual void Capture() { }

    public virtual void Clear()
    {
        //this.ViewModel.Strokes.Clear();
        this.ViewModel.SignatureImage = null;
    }
}
