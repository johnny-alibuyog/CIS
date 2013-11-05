using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        //public static Bitmap BitmapToGrayscale(Bitmap source)
        //{
        //    // Create target image.
        //    var width = source.Width;
        //    var height = source.Height;
        //    var target = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

        //    // Set the palette to discrete shades of gray
        //    var palette = target.Palette;
        //    for (int i = 0; i < palette.Entries.Length; i++)
        //    {
        //        palette.Entries[i] = Color.FromArgb(0, i, i, i);
        //    }
        //    target.Palette = palette;

        //    // Lock bits so we have direct access to bitmap data
        //    var targetData = target.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
        //    var sourceData = source.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

        //    unsafe
        //    {
        //        for (int r = 0; r < height; r++)
        //        {
        //            byte* pTarget = (byte*)(targetData.Scan0 + r * targetData.Stride);
        //            byte* pSource = (byte*)(sourceData.Scan0 + r * sourceData.Stride);
        //            for (int c = 0; c < width; c++)
        //            {
        //                byte colorIndex = (byte)(((*pSource) * 0.3 + *(pSource + 1) * 0.59 + *(pSource + 2) * 0.11));
        //                *pTarget = colorIndex;
        //                pTarget++;
        //                pSource += 3;
        //            }
        //        }
        //    }

        //    target.UnlockBits(targetData);
        //    source.UnlockBits(sourceData);

        //    return target;
        //}

        //public static Bitmap BitmapToGrayscale4bpp(this Bitmap source)
        //{
        //    // Create target image.
        //    var width = source.Width;
        //    var height = source.Height;
        //    var target = new Bitmap(width, height, PixelFormat.Format4bppIndexed);

        //    // Set the palette to discrete shades of gray
        //    var palette = target.Palette;

        //    for (int i = 0; i < palette.Entries.Length; i++)
        //    {
        //        var cval = 17 * i;
        //        palette.Entries[i] = Color.FromArgb(0, cval, cval, cval);
        //    }

        //    target.Palette = palette;

        //    // Lock bits so we have direct access to bitmap data
        //    var targetData = target.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format4bppIndexed);
        //    var sourceData = source.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

        //    unsafe
        //    {
        //        for (int r = 0; r < height; r++)
        //        {
        //            byte* pTarget = (byte*)(targetData.Scan0 + r * targetData.Stride);
        //            byte* pSource = (byte*)(sourceData.Scan0 + r * sourceData.Stride);
        //            byte prevValue = 0;
        //            for (int c = 0; c < width; c++)
        //            {
        //                byte colorIndex = (byte)((((*pSource) * 0.3 + *(pSource + 1) * 0.59 + *(pSource + 2) * 0.11)) / 16);
        //                if (c % 2 == 0)
        //                    prevValue = colorIndex;
        //                else
        //                    *(pTarget++) = (byte)(prevValue | colorIndex << 4);
        //                    //*(pTarget++) = (byte)(prevValue << 4 | colorIndex);

        //                pSource += 3;
        //            }
        //        }
        //    }

        //    target.UnlockBits(targetData);
        //    source.UnlockBits(sourceData);

        //    return target;
        //}

        public static Bitmap ReduceSize(this Bitmap source)
        {
            using (var sourceStream = new MemoryStream())
            using (var targetStream = new MemoryStream())
            {
                source.Save(sourceStream, source.RawFormat);
                ResizeImage(100D, sourceStream, targetStream);
                return new Bitmap(targetStream);
            }
        }

        private static void ResizeImage(double scaleFactor, Stream fromStream, Stream toStream)
        {
            using (var image = Image.FromStream(fromStream))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);

                using (var thumbnailBitmap = new Bitmap(newWidth, newHeight))
                using (var thumbnailGraph = Graphics.FromImage(thumbnailBitmap))
                {
                    thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    
                    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    thumbnailGraph.DrawImage(image, imageRectangle);
                    thumbnailBitmap.Save(toStream, image.RawFormat);
                }
            }


            //var image = Image.FromStream(fromStream);
            //var newWidth = (int)(image.Width * scaleFactor);
            //var newHeight = (int)(image.Height * scaleFactor);
            //var thumbnailBitmap = new Bitmap(newWidth, newHeight);

            //var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            //thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            //thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            //thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            //thumbnailGraph.DrawImage(image, imageRectangle);

            //thumbnailBitmap.Save(toStream, image.RawFormat);

            //thumbnailGraph.Dispose();
            //thumbnailBitmap.Dispose();
            //image.Dispose();
        }
    }
}
