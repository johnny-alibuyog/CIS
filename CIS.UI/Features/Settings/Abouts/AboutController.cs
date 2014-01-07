using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;

namespace CIS.UI.Features.Settings.Abouts
{
    public class AboutController : ControllerBase<AboutViewModel>
    {
        public AboutController(AboutViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.About = App.Data.Product.AboutTemplate.Replace("LICENSEE", App.Data.Product.Licensee);
        }
    }
}
