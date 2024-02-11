using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Common.Signature
{
    /// <summary>
    /// Interaction logic for SignatureView.xaml
    /// </summary>
    public partial class SignatureView : UserControl, IViewFor<SignatureViewModel>
    {
        #region IViewFor<ApplicationViewModel> Members

        public SignatureViewModel ViewModel
        {
            get { return this.DataContext as SignatureViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        #region Routine Helpers

        private byte[] GetBitmapBytes(InkCanvas inkCanvas)
        {
            //get the dimensions of the ink control
            var margin = (int)inkCanvas.Margin.Left;
            var width = (int)inkCanvas.ActualWidth - margin;
            var height = (int)inkCanvas.ActualHeight - margin;

            //render ink to bitmap
            var target = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
            target.Render(inkCanvas);

            //save the ink to a memory stream
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(target));

            //get the bitmap bytes from the memory stream
            var bitmapBytes = (byte[])null;
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Position = 0;
                bitmapBytes = stream.ToArray();
            }

            return bitmapBytes;
        }

        private BitmapSource GetBitmapSource(InkCanvas inkCanvas)
        {
            //get the dimensions of the ink control
            var margin = (int)inkCanvas.Margin.Left;
            var width = (int)inkCanvas.ActualWidth - margin / 2;
            var height = (int)inkCanvas.ActualHeight - margin / 2;

            //render ink to bitmap
            var renderTarget = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
            var visual = new DrawingVisual();
            using (var context = visual.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(inkCanvas);
                context.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), new System.Windows.Size(width, height)));
            }
            renderTarget.Render(visual);

            //save the ink to a memory stream
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTarget));

            //get the bitmap bytes from the memory stream
            var bitmapImage = new BitmapImage();
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }

        #endregion

        public SignatureView()
        {
            InitializeComponent();
        }

        private void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            this.SignatureImage.Source = GetBitmapSource(this.SignatureInkCanvas);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.SignatureImage.Source = null;
            this.SignatureInkCanvas.Strokes.Clear();
        }
    }
}
