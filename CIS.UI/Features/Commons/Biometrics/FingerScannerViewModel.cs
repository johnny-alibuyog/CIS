using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Commons.Biometrics
{
    public class FingerScannerViewModel : ViewModelBase
    {
        private FingerScannerController _controller;

        public virtual Dictionary<Finger, BitmapSource> FingerImages { get; set; }

        public virtual BitmapSource CapturedFingerImage { get; set; }

        public virtual Finger CurrentFinger { get; set; }

        public virtual ReactiveCollection<string> EventLogs { get; set; }

        public virtual string Prompt { get; set; }

        public virtual string Status { get; set; }

        public virtual IReactiveCommand Stop { get; set; }

        public virtual IReactiveCommand Start { get; set; }

        public FingerScannerViewModel()
        {
            this.EventLogs = new ReactiveCollection<string>();
            this.CurrentFinger = Finger.RightThumb;
            this.FingerImages = new Dictionary<Finger, BitmapSource>()
                {
                    { Finger.RightThumb, (BitmapSource)null },
                    { Finger.RightIndex, (BitmapSource)null },
                    { Finger.RightMiddle, (BitmapSource)null },
                    { Finger.RightRing, (BitmapSource)null },
                    { Finger.RightPinky, (BitmapSource)null },
                    { Finger.LeftThumb, (BitmapSource)null },
                    { Finger.LeftIndex, (BitmapSource)null },
                    { Finger.LeftMiddle, (BitmapSource)null },
                    { Finger.LeftRing, (BitmapSource)null },
                    { Finger.LeftPinky, (BitmapSource)null },
                };

            _controller = new FingerScannerController(this);
        }
    }
}
