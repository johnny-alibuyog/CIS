using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Features.Commons.Biometrics;
using CIS.UI.Features.Commons.Cameras;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Clearances
{
    public class ApplicationViewModel : ViewModelBase
    {
        private readonly ApplicationController _controller;

        public virtual PersonalInformationViewModel PersonalInformation { get; set; }

        public virtual CameraViewModel Camera { get; set; }

        public virtual FingerScannerViewModel FingerScanner { get; set; }

        public virtual SummaryViewModel Summary { get; set; }

        public virtual IList<ViewModelBase> ViewModels { get; set; }

        public virtual ViewModelBase CurrentViewModel { get; set; }

        public virtual IReactiveCommand Previous { get; set; }

        public virtual IReactiveCommand Next { get; set; }

        public virtual IReactiveCommand Reset { get; set; }

        public virtual IReactiveCommand Release { get; set; }

        public virtual IReactiveCommand Print { get; set; }

        public ApplicationViewModel()
        {
            _controller = new ApplicationController(this);
        }
    }
}
