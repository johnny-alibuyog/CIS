using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Commons
{
    public class ImageScaleFactorConfiguration : Configuration
    {
        public virtual double DefaultResizeScaleFactor 
        {
            get { return double.Parse(this.GetPropertyValue("DefaultResizeScaleFactor")); } 
            set { this.SetPropertyValue("DefaultResizeScaleFactor", value.ToString()); } 
        }

        public virtual double PictureResizeScaleFactor
        {
            get { return double.Parse(this.GetPropertyValue("PictureResizeScaleFactor")); }
            set { this.SetPropertyValue("PictureResizeScaleFactor", value.ToString()); }
        }

        public virtual double FingerPrintResizeScaleFactor
        {
            get { return double.Parse(this.GetPropertyValue("FingerPrintResizeScaleFactor")); }
            set { this.SetPropertyValue("FingerPrintResizeScaleFactor", value.ToString()); }
        }

        public ImageScaleFactorConfiguration()
        {
            this.DefaultResizeScaleFactor = 0.25D;
            this.PictureResizeScaleFactor = 0.25D;
            this.FingerPrintResizeScaleFactor = 0.25D; //0.20D;
        }
    }
}
