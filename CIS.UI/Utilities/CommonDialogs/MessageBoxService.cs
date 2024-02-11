using System;
using System.Windows;
using Common.Logging;
using FirstFloor.ModernUI.Windows.Controls;

namespace CIS.UI.Utilities.CommonDialogs;

public class MessageBoxService : IMessageBoxService
{
    private static readonly ILog _log = LogManager.GetCurrentClassLogger();

    public void Warn(string message, string caption = "Error")
    {
        Application.Current.Dispatcher.Invoke(() => ModernDialog.ShowMessage(message, caption, MessageBoxButton.OK));
    }

    public void Warn(string message, Exception ex, string caption = "Error")
    {
        _log.Error(ex.Message, ex);
        Application.Current.Dispatcher.Invoke(() => ModernDialog.ShowMessage(message, caption, MessageBoxButton.OK));
    }

    public bool? Confirm(string message, string caption = "Confirmation", bool withCancel = false)
    {
        var button = withCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo;
        var result = Application.Current.Dispatcher.Invoke(() => ModernDialog.ShowMessage(message, caption, button));
        return result switch
        {
            MessageBoxResult.Yes => true,
            MessageBoxResult.No  => false,
            _                    => null,
        };
    }

    public void Inform(string message, string caption = "Information")
    {
        Application.Current.Dispatcher.Invoke(() => ModernDialog.ShowMessage(message, caption, MessageBoxButton.OK));
    }
}
