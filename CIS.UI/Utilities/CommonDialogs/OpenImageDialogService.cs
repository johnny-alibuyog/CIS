using System;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace CIS.UI.Utilities.CommonDialogs;

public class OpenImageDialogService : IOpenImageDialogService
{
    public BitmapImage Show()
    {
        var dialog = new OpenFileDialog()
        {
            Title = "Select a picture",
            Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png"
        };

        var result = dialog.ShowDialog();

        return (result == true) ? new BitmapImage(new Uri(dialog.FileName)) : null;
    }
}
