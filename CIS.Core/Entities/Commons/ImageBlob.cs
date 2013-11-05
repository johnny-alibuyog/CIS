using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace CIS.Core.Entities.Commons
{
    public class ImageBlob : Blob
    {
        private Image _image;

        public virtual Image Image
        {
            get
            {
                if (_image != null)
                    return _image;

                if (this.Bytes != null)
                {
                    using (var stream = new MemoryStream(this.Bytes))
                    {
                        _image = new Bitmap(Image.FromStream(stream));
                    }
                }
                return _image;
            }
            set
            {
                _image = value;
                if (_image == null)
                {
                    this.Bytes = null;
                    return;
                }

                using (var stream = new MemoryStream())
                {
                    _image.Save(stream, ImageFormat.Bmp);
                    this.Bytes = stream.ToArray();
                }
            }
        }

        #region Constructors

        public ImageBlob() { }

        public ImageBlob(Image content)
        {
            this.Image = content;
        }

        #endregion
    }
}
