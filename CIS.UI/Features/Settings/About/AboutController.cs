namespace CIS.UI.Features.Settings.About;

public class AboutController : ControllerBase<AboutViewModel>
{
    public AboutController(AboutViewModel viewModel) : base(viewModel)
    {
        this.ViewModel.About = App.Context.Product.AboutTemplate.Replace("LICENSEE", App.Context.Product.Licensee);
    }
}
