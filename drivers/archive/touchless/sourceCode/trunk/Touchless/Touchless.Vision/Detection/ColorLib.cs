using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Touchless.Vision.Detection
{
    /// <summary>
    /// Represents a color in RGB (Red [0-255], Green [0-255], Blue [0-255])
    /// </summary>
    internal struct RGB
    {
        public static byte RGB_MAX_R = 255;
        public static byte RGB_MAX_G = 255;
        public static byte RGB_MAX_B = 255;

        /// <summary>
        /// Create a new RGB (Red, Green, Blue) struct
        /// </summary>
        /// <param name="r">RGB R [0-255]</param>
        /// <param name="g">RGB G [0-255]</param>
        /// <param name="b">RGB B [0-255]</param>
        public RGB(byte r, byte g, byte b) { R = r; G = g; B = b; }

        /// <summary>
        /// Create a new RGB (Red, Green, Blue) struct from an ARGB int
        /// </summary>
        /// <param name="argb">ARGB integer in the format 0xAARRGGBB</param>
        public RGB(int argb)
        {
            this.B = (byte)(argb & 0xff);
            this.G = (byte)((argb >>= 8) & 0xff);
            this.R = (byte)((argb >> 8) & 0xff);
        }

        #region Static Conversion Methods

        /// <summary>
        /// Convert a System.Drawing.Color to its corresponding RGB
        /// </summary>
        /// <param name="c">A System.Drawing.Color</param>
        /// <returns>A corresponding RGB structure</returns>
        public static RGB ConvertFromColor(Color c) { return new RGB(c.R, c.G, c.B); }

        /// <summary>
        /// Convert an RGB structure to its corresponding System.Drawing.Color
        /// </summary>
        /// <param name="rgb">An RGB structure</param>
        /// <returns>A corresponding System.Drawing.Color</returns>
        public static Color ConvertToColor(RGB rgb) { return Color.FromArgb(rgb.R, rgb.G, rgb.B); }

        /// <summary>
        /// Convert an RGB structure to its corresponding HSV structure
        /// </summary>
        /// <param name="rgb">An RGB structure</param>
        /// <returns>A corresponding HSV structure</returns>
        public static HSV ConvertToHSV(RGB rgb)
        {
            HSV hsv = new HSV();
            byte min, max;

            // Find the min / max for r, g, b
            if (rgb.R <= rgb.G)
            {
                if (rgb.R >= rgb.B) { min = rgb.B; max = rgb.G; }
                else { min = rgb.R; max = (rgb.G >= rgb.B) ? rgb.G : rgb.B; }
            }
            else if (rgb.R <= rgb.B) { min = rgb.G; max = rgb.B; }
            else { max = rgb.R; min = (rgb.G <= rgb.B) ? rgb.G : rgb.B; }

            // HSV value and saturation
            hsv.V = max;
            hsv.S = (byte)((max == 0) ? 0 : 255 * (max - min) / max);

            // HSV hue
            if (max == min) hsv.H = 0;
            else
            {
                if (max == rgb.R) hsv.H = (short)((60 * (rgb.G - rgb.B) / (max - min) + 360) % 360);
                else if (max == rgb.G) hsv.H = (short)(60 * (rgb.B - rgb.R) / (max - min) + 120);
                else if (max == rgb.B) hsv.H = (short)(60 * (rgb.R - rgb.G) / (max - min) + 240);
            }

            return hsv;
        }

        #endregion

        public override string ToString() { return String.Format("RGB ({0}, {1}, {2}) - {3}", R, G, B, ConvertToColor(this)); }

        // RGB R [0-255], G [0-255], B [0-255]
        public byte R, G, B;
    }


    /// <summary>
    /// Represents a color in HSV (Hue [0-360], Saturation [0-255], Value [0-255])
    /// </summary>
    internal struct HSV
    {
        public static short HSV_MAX_H = 360;
        public static byte HSV_MAX_S = 255;
        public static byte HSV_MAX_V = 255;

        /// <summary>
        /// Create a new HSV (Hue, Saturation, Value) struct
        /// </summary>
        /// <param name="h">HSV H [0-360]</param>
        /// <param name="s">HSV S [0-255]</param>
        /// <param name="v">HSV V [0-255]</param>
        public HSV(short h, byte s, byte v) { H = h; S = s; V = v; }

        #region Static Conversion Methods

        /// <summary>
        /// Convert a System.Drawing.Color to its corresponding HSV
        /// </summary>
        /// <param name="c">A System.Drawing.Color</param>
        /// <returns>A corresponding HSV structure</returns>
        public static HSV ConvertFromColor(Color c) { return RGB.ConvertToHSV(new RGB(c.R, c.G, c.B)); }

        /// <summary>
        /// Convert an HSV structure to its corresponding System.Drawing.Color
        /// </summary>
        /// <param name="hsv">An HSV structure</param>
        /// <returns>A corresponding System.Drawing.Color</returns>
        public static System.Drawing.Color ConvertToColor(HSV hsv)
        {
            RGB rgb = HSV.ConvertToRGB(hsv);
            return Color.FromArgb(rgb.R, rgb.G, rgb.B);
        }

        /// <summary>
        /// Convert an HSV structure to its corresponding RGB structure
        /// </summary>
        /// <param name="hsv">An HSV structure</param>
        /// <returns>A corresponding RGB structure</returns>
        public static RGB ConvertToRGB(HSV hsv)
        {
            int h = (hsv.H / 60) % 6;
            float f = hsv.H / 60.0F - hsv.H / 60;
            byte p = (byte)((hsv.V * (HSV.HSV_MAX_S - hsv.S)) / HSV.HSV_MAX_S);
            byte q = (byte)((hsv.V * (HSV.HSV_MAX_S - f * hsv.S)) / HSV.HSV_MAX_S);
            byte t = (byte)((hsv.V * (HSV.HSV_MAX_S - (1 - f) * hsv.S)) / HSV.HSV_MAX_S);
            if (h == 0) return new RGB(hsv.V, t, p);
            else if (h == 1) return new RGB(q, hsv.V, p);
            else if (h == 2) return new RGB(p, hsv.V, t);
            else if (h == 3) return new RGB(p, q, hsv.V);
            else if (h == 4) return new RGB(t, p, hsv.V);
            else if (h == 5) return new RGB(hsv.V, p, q);
            return new RGB();
        }

        /// <summary>
        /// Returns an HSV representing the binned version of the argument HSV color
        /// </summary>
        /// <param name="hsv">The HSV color to be binned and then used to generate the hash key</param>
        /// <param name="binCounts">The count of bins per Hue, Sat, Val space</param>
        public static HSV GetBinnedHSV(HSV hsv, HSV binCounts)
        {
            HSV binnedHSV = new HSV();
            binnedHSV.H = (short)(hsv.H * (binCounts.H - 1) / HSV.HSV_MAX_H);
            binnedHSV.S = (byte)(hsv.S * (binCounts.S - 1) / HSV.HSV_MAX_S);
            binnedHSV.V = (byte)(hsv.V * (binCounts.V - 1) / HSV.HSV_MAX_V);
            return binnedHSV;
        }

        #endregion

        public override string ToString() { return String.Format("HSV ({0}, {1}, {2}) - {3}", H, S, V, ConvertToColor(this)); }

        // HSV H [0-360], S [0-255], V [0-255]
        public short H;
        public byte S, V;
    }

    /// <summary>
    /// A hash key that represents a color
    /// </summary>
    internal struct ColorKey : IComparable<ColorKey>
    {
        /// <summary>
        /// The RGB value used to generate the hash key
        /// </summary>
        public RGB Rgb { set { key = value.R + value.G * RGB.RGB_MAX_R + value.B * RGB.RGB_MAX_R * RGB.RGB_MAX_G; } }

        /// <summary>
        /// The HSV value used to generate the hash key
        /// </summary>
        public HSV Hsv { set { key = value.H + value.S * HSV.HSV_MAX_H + value.V * HSV.HSV_MAX_S * HSV.HSV_MAX_H; } }

        /// <summary>
        /// Compares the current object with another object of the same type
        /// Implemented for use with SortedDictionary
        /// </summary>
        /// <param name="other">An object to compare with this object</param>
        /// <returns>LT 0 (this LT other), 0 (equal), GT 0 (this GT other)</returns>
        int IComparable<ColorKey>.CompareTo(ColorKey other) { return key - other.key; }

        private int key;
    }
}
