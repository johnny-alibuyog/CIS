using System;

namespace CIS.UI.Utilities.CommonDialogs;

public interface IMessageBoxService
{
    void Warn(string message, string caption = "Warning");
    void Warn(string message, Exception ex, string caption = "Warning");
    void Inform(string message, string caption = "Information");
    bool? Confirm(string message, string caption = "Confirmation", bool withCancel = false);
}
