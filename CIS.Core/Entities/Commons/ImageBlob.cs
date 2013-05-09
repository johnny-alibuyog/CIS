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
        private Image _content;

        public virtual Image Content
        {
            get
            {
                if (_content != null)
                    return _content;

                if (this.Data != null)
                {
                    using (var ms = new MemoryStream(this.Data))
                    {
                        _content = new Bitmap(Image.FromStream(ms));
                    }
                }
                return _content;
            }
            set
            {
                _content = value;
                if (_content == null)
                {
                    this.Data = null;
                    return;
                }

                using (var stream = new MemoryStream())
                {
                    _content.Save(stream, ImageFormat.Bmp);
                    this.Data = stream.ToArray();
                }
            }
        }

        #region Constructors

        public ImageBlob() { }

        public ImageBlob(Image content)
        {
            this.Content = content;
        }

        #endregion
    }
}
