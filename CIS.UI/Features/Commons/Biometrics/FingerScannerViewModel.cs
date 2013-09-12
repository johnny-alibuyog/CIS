using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Commons.Biometrics
{
    public class FingerScannerViewModel : ViewModelBase
    {
        private FingerScannerController _controller;

        public virtual Dictionary<FingerViewModel, BitmapSource> FingerImages { get; set; }

        [NotNull(Message = "Finger print is mandatory.")]
        public virtual BitmapSource CapturedFingerImage { get; set; }

        public virtual FingerViewModel CurrentFinger { get; set; }

        public virtual IReactiveList<string> EventLogs { get; set; }

        public virtual string Prompt { get; set; }

        public virtual string Status { get; set; }

        public virtual IReactiveCommand Stop { get; set; }

        public virtual IReactiveCommand Start { get; set; }

        public FingerScannerViewModel()
        {
            this.EventLogs = new ReactiveList<string>();
            this.CurrentFinger = FingerViewModel.RightThumb;
            this.FingerImages = new Dictionary<FingerViewModel, BitmapSource>()
                {
                    { FingerViewModel.RightThumb, (BitmapSource)null },
                    { FingerViewModel.RightIndex, (BitmapSource)null },
                    { FingerViewModel.RightMiddle, (BitmapSource)null },
                    { FingerViewModel.RightRing, (BitmapSource)null },
                    { FingerViewModel.RightPinky, (BitmapSource)null },
                    { FingerViewModel.LeftThumb, (BitmapSource)null },
                    { FingerViewModel.LeftIndex, (BitmapSource)null },
                    { FingerViewModel.LeftMiddle, (BitmapSource)null },
                    { FingerViewModel.LeftRing, (BitmapSource)null },
                    { FingerViewModel.LeftPinky, (BitmapSource)null },
                };

            //_controller = new FingerScannerController(this);
            _controller = IoC.Container.Resolve<FingerScannerController>(new ViewModelDependency(this));

        }
    }
}
