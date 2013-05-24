using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media.Imaging;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Commons.Signatures
{
    public class SignatureViewModel : ViewModelBase
    {
        public virtual StrokeCollection Strokes { get; private set; }

        public virtual BitmapSource Signature { get; set; }

        public virtual IReactiveCommand Capture { get; set; }

        public virtual IReactiveCommand Clear { get; set; }

        public SignatureViewModel()
        {
            Strokes = new StrokeCollection();
        }
    }
}
