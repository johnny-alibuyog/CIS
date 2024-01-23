using System;
using ZXing;
using ZXing.Common;

namespace CIS.Core.Entities.Commons;

public class Barcode : Entity<Guid>
{
    private ImageBlob _image;
    private string _text;

    public virtual ImageBlob Image
    {
        get => _image;
        protected set => _image = value;
    }

    public virtual string Text
    {
        get => _text;
        protected set => _text = value;
    }

    private static readonly Random _random = new();

    public static Barcode GenerateBarcode()
    {
        var barcodeText = Barcode.GeneratBarcodeText();
        var barcodeImage = Barcode.GenerateBarcodeImage(barcodeText);
        return new Barcode()
        {
            Text = barcodeText,
            Image = barcodeImage
        };
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
}
