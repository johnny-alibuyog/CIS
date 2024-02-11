using System;
using System.Windows.Media.Imaging;

namespace CIS.UI.Features.Common.Biometrics;

public class FingerPrintViewModel : ViewModelBase
{
    public virtual Guid Id { get; set; }

    public virtual BitmapSource RightThumb { get; set; }

    public virtual BitmapSource RightIndex { get; set; }

    public virtual BitmapSource RightMiddle { get; set; }

    public virtual BitmapSource RightRing { get; set; }

    public virtual BitmapSource RightPinky { get; set; }

    public virtual BitmapSource LeftThumb { get; set; }

    public virtual BitmapSource LeftIndex { get; set; }

    public virtual BitmapSource LeftMiddle { get; set; }

    public virtual BitmapSource LeftRing { get; set; }

    public virtual BitmapSource LeftPinky { get; set; }
}
