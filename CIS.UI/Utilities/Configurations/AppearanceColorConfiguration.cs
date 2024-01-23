using System;
using System.Windows.Media;
using System.Xml.Serialization;

namespace CIS.UI.Utilities.Configurations
{
    [Serializable()]
    public class AppearanceColorConfiguration
    {
        public static readonly AppearanceColorConfiguration Empty = new AppearanceColorConfiguration();

        private byte _red;
        private byte _green;
        private byte _blue;

        public virtual byte Red
        {
            get { return _red; }
            set { _red = value; }
        }

        public virtual byte Green
        {
            get { return _green; }
            set { _green = value; }
        }

        public virtual byte Blue
        {
            get { return _blue; }
            set { _blue = value; }
        }

        [XmlIgnore]
        public virtual Color Value
        {
            get
            {
                return Color.FromRgb(
                    r: _red, 
                    g: _green, 
                    b: _blue
                );
            }
            set
            {
                _red = value.R;
                _green = value.G;
                _blue = value.B;
            }
        }

        #region Equality Comparer

        public override bool Equals(object obj)
        {
            var that = obj as AppearanceColorConfiguration;

            if (that == null)
                return false;

            if (that.Red != this.Red)
                return false;

            if (that.Green != this.Green)
                return false;

            if (that.Blue != this.Blue)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 23 + (this.Red != default(byte) ? this.Red.GetHashCode() : 0);
            hashCode = hashCode * 23 + (this.Green != default(byte) ? this.Green.GetHashCode() : 0);
            hashCode = hashCode * 23 + (this.Blue != default(byte) ? this.Blue.GetHashCode() : 0);

            return hashCode;
        }

        public static bool operator ==(AppearanceColorConfiguration x, AppearanceColorConfiguration y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(AppearanceColorConfiguration x, AppearanceColorConfiguration y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
