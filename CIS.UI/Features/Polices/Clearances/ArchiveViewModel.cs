using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Clearances
{
    public class ArchiveViewModel : ViewModelBase
    {
        private readonly ArchiveController _controller;

        public virtual ArchiveCriteriaViewModel Criteria { get; set; }

        public virtual IList<ArchiveItemViewModel> Items { get; set; }

        public virtual IReactiveCommand Search { get; set; }

        public virtual IReactiveCommand ViewItem { get; set; }

        public virtual IReactiveCommand ViewReport { get; set; }

        public ArchiveViewModel()
        {
            this.Criteria = new ArchiveCriteriaViewModel();

            _controller = new ArchiveController(this);
        }
    }
}
