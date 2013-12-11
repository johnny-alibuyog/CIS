using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Utilities.Configurations
{
    [Serializable()]
    public class ImageScaleFactorConfiguration
    {
        public virtual double DefaultResizeScaleFactor { get; set; }
        public virtual double PictureResizeScaleFactor { get; set; }
        public virtual double FingerPrintResizeScaleFactor { get; set; }

        public ImageScaleFactorConfiguration()
        {
            this.DefaultResizeScaleFactor = 0.25D;
            this.PictureResizeScaleFactor = 0.25D;
            this.FingerPrintResizeScaleFactor = 0.25D; //0.20D;
        }
    }
}
