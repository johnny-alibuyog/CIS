namespace CIS.Core.Domain.Common;

public class ImageScaleFactorConfiguration : Configuration
{
    public virtual double DefaultResizeScaleFactor
    {
        get => double.Parse(this.GetPropertyValue("DefaultResizeScaleFactor"));
        set => this.SetPropertyValue("DefaultResizeScaleFactor", value.ToString());
    }

    public virtual double PictureResizeScaleFactor
    {
        get => double.Parse(this.GetPropertyValue("PictureResizeScaleFactor"));
        set => this.SetPropertyValue("PictureResizeScaleFactor", value.ToString());
    }

    public virtual double FingerPrintResizeScaleFactor
    {
        get => double.Parse(this.GetPropertyValue("FingerPrintResizeScaleFactor"));
        set => this.SetPropertyValue("FingerPrintResizeScaleFactor", value.ToString());
    }

    public ImageScaleFactorConfiguration()
    {
        this.DefaultResizeScaleFactor = 0.25D;
        this.PictureResizeScaleFactor = 0.25D;
        this.FingerPrintResizeScaleFactor = 0.25D; //0.20D;
    }
}
