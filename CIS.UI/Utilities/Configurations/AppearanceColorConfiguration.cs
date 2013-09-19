using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using FirstFloor.ModernUI.Presentation;

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

        private Nullable<int> _hashCode;

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
            if (_hashCode == null)
            {
                unchecked
                {
                    _hashCode = 17;
                    _hashCode = _hashCode * 23 + (this.Red != default(byte) ? this.Red.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.Green != default(byte) ? this.Green.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (this.Blue != default(byte) ? this.Blue.GetHashCode() : 0);
                }
             }

            return _hashCode.Value;
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
