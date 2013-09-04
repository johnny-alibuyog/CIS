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
            var result = ModernDialog.ShowMessage(text, title, button);
            switch (result)
            {
                case MessageBoxResult.OK:
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                case MessageBoxResult.Cancel:
                    return false;
                case MessageBoxResult.None:
                    return null;
                default:
                    return null;
            }
        }
    }
}
