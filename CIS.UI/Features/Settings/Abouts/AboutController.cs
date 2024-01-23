namespace CIS.UI.Features.Settings.Abouts;

public class AboutController : ControllerBase<AboutViewModel>
{
    public AboutController(AboutViewModel viewModel) : base(viewModel)
    {
        this.ViewModel.About = App.Data.Product.AboutTemplate.Replace("LICENSEE", App.Data.Product.Licensee);
    }
}
