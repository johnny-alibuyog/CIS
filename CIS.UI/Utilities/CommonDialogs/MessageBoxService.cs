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
        private readonly ILog _log = LogManager.GetCurrentClassLogger();

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

        public bool Confirm(string message, string caption = "Confirmation")
        {
            //var result = ModernDialog.ShowMessage(message, caption, MessageBoxButton.YesNo);
            var result = Application.Current.Dispatcher.Invoke(() => ModernDialog.ShowMessage(message, caption, MessageBoxButton.YesNo));
            return (result == MessageBoxResult.Yes);
        }

        public void Inform(string message, string caption = "Information")
        {
            Application.Current.Dispatcher.Invoke(() => ModernDialog.ShowMessage(message, caption, MessageBoxButton.OK));
            //ModernDialog.ShowMessage(message, caption, MessageBoxButton.OK);
        }
    }
}
