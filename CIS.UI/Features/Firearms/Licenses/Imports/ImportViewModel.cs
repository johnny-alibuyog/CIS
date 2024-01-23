using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;
using System;

namespace CIS.UI.Features.Firearms.Licenses.Imports
{
    public class ImportViewModel : ViewModelBase
    {
        private readonly ImportController _controller;

        public virtual string SourcePath { get; set; }

        public virtual DateTime? ImportStart { get; set; }

        public virtual DateTime? ImportEnd { get; set; }

        public virtual TimeSpan? TotalTime { get; set; }

        public virtual decimal? TotalLicenses { get; set; }

        public virtual string Status { get; set; }

        public virtual string ErrorMessage { get; set; }

        public virtual IReactiveCommand LookupPath { get; set; }

        public virtual IReactiveCommand Reset { get; set; }

        public virtual IReactiveCommand Import { get; set; }

        public ImportViewModel()
        {
            //_controller = new ImportController(this);
            _controller = IoC.Container.Resolve<ImportController>(new ViewModelDependency(this));
        }
    }
}
