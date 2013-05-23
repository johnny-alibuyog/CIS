using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using ZXing;
using ZXing.Common;

namespace CIS.Core.Entities.Commons
{
    public class Barcode
    {
        private Guid _id;
        private ImageBlob _image;
        private string _text;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual ImageBlob Image
        {
            get { return _image; }
            protected set { _image = value; }
        }

        public virtual string Text
        {
            get { return _text; }
            protected set { _text = value; }
        }

        #region Constructors

        public Barcode() { }

        public Barcode(string text)
        {
            _text = text;
            _image = Barcode.GenerateBarcodeImage(text);
        }

        public Barcode(string text, ImageBlob image)
        {
            _text = text;
            _image = image;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Barcode;

            if (that == null)
                return false;

            if (that.Id == Guid.Empty && this.Id == Guid.Empty)
                return object.ReferenceEquals(that, this);

            return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (this.Id != Guid.Empty)
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Barcode x, Barcode y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Barcode x, Barcode y)
        {
            return !Equals(x, y);
        }

        #endregion

        #region Static Members

        private static readonly Random _random = new Random();

        public static Barcode GenerateBarcode()
        {
            return new Barcode(Barcode.GeneratBarcodeText());
        }

        public static string GeneratBarcodeText()
        {
            var text = string.Empty;
            text += _random.Next(Guid.NewGuid().GetHashCode(), int.MaxValue).ToString();
            text += _random.Next(Guid.NewGuid().GetHashCode(), int.MaxValue).ToString();
            return text;
        }

        public static ImageBlob GenerateBarcodeImage(string text)
        {
            var builder = new BarcodeWriter()
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions()
                {
                    PureBarcode = true,
                    Height = 50,
                    Width = 150,
                },
            };
            return new ImageBlob(builder.Write(text));
        }

        #endregion
    }
}
