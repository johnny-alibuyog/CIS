using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class ImportViewModel : ViewModelBase
    {
        private readonly ImportController _controller;

        public virtual string SourcePath { get; set; }

        public virtual Nullable<DateTime> ImportStart { get; set; }

        public virtual Nullable<DateTime> ImportEnd { get; set; }

        public virtual Nullable<TimeSpan> TotalTime { get; set; }

        public virtual Nullable<decimal> TotalLicenses { get; set; }

        public virtual string Status { get; set; }

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
