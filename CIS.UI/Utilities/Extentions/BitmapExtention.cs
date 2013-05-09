using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CIS.UI.Utilities.Extentions
{
    public static class BitmapExtention
    {
        /// <summary>
        /// Converts wpf bitmap to win forms bitmap
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        public static Image ToImage(this BitmapSource bitmapSource)
        {
            if (bitmapSource == null)
                return null;

            using (var stream = new MemoryStream())
            {
                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);

                using (var tempBitmap = new Bitmap(stream))
                {
                    // According to MSDN, one "must keep the stream open for the lifetime of the Bitmap."
                    // So we return a copy of the new bitmap, allowing us to dispose both the bitmap and the stream.
                    return new Bitmap(tempBitmap);
                }
            }
        }

        /// <summary>
        /// Converts win forms bitmap to wpf bitmap
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static BitmapSource ToBitmapSource(this Image image)
        {
            if (image == null)
                return null;

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Bmp);
                stream.Position = 0;

                var result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
    }
}
