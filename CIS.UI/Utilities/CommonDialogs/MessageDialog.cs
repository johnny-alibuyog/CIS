using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;

namespace CIS.UI.Utilities.CommonDialogs
{
    public static class MessageDialog
    {
        public static bool? Show(string text, string title, MessageBoxButton button)
        {
            return ModernDialog.ShowMessage(text, title, button);
        }
    }
}
