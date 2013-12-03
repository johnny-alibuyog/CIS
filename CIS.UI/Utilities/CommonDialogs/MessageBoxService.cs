using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Common.Logging;
using FirstFloor.ModernUI.Windows.Controls;

namespace CIS.UI.Utilities.CommonDialogs
{
    public class MessageBoxService : IMessageBoxService
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        public void Warn(string message, string caption = "Error")
        {
            Application.Current.Dispatcher.Invoke(() => ModernDialog.ShowMessage(message, caption, MessageBoxButton.OK));
            //ModernDialog.ShowMessage(message, caption, MessageBoxButton.OK);
        }

        public void Warn(string message, Exception ex, string caption = "Error")
        {
            _log.Error(ex.Message, ex);
            Application.Current.Dispatcher.Invoke(() => ModernDialog.ShowMessage(message, caption, MessageBoxButton.OK));
            //ModernDialog.ShowMessage(message, caption, MessageBoxButton.OK);
        }

        public Nullable<bool> Confirm(string message, string caption = "Confirmation", bool withCancel = false)
        {
            var button = withCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo;
            //var result = ModernDialog.ShowMessage(message, caption, button);
            var result = Application.Current.Dispatcher.Invoke(() => ModernDialog.ShowMessage(message, caption, button));
            switch (result)
            {
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return null;
            }
        }

        public void Inform(string message, string caption = "Information")
        {
            Application.Current.Dispatcher.Invoke(() => ModernDialog.ShowMessage(message, caption, MessageBoxButton.OK));
            //ModernDialog.ShowMessage(message, caption, MessageBoxButton.OK);
        }
    }
}
