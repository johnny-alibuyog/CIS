using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Settings.Abouts
{
    public class AboutController : ControllerBase<AboutViewModel>
    {
        public AboutController(AboutViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.About = App.Config.Product.AboutTemplate.Replace("LICENSEE", App.Config.Product.Licensee);
        }
    }
}
