using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Clearances
{
    public class ArchiveViewModel : ViewModelBase
    {
        private readonly ArchiveController _controller;

        public virtual ArchiveCriteriaViewModel Criteria { get; set; }

        public virtual IList<ArchiveItemViewModel> Items { get; set; }

        public virtual IReactiveCommand Search { get; set; }

        public virtual IReactiveCommand ViewApplicant { get; set; }

        public virtual IReactiveCommand ViewClearance { get; set; }

        public virtual IReactiveCommand GenerateListReport { get; set; }

        public ArchiveViewModel()
        {
            //_controller = new ArchiveController(this);
            _controller = IoC.Container.Resolve<ArchiveController>(new ViewModelDependency(this));
        }
    }
}
