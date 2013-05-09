using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace CIS.UI.Utilities.CommonDialogs
{
    public static class OpenImageDialog
    {
        public static BitmapImage Show()
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                    "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                    "Portable Network Graphic (*.png)|*.png"
            };

            var result = dialog.ShowDialog();
            if (result == true)
                return new BitmapImage(new Uri(dialog.FileName));
            else
                return null;
        }
    }
}
