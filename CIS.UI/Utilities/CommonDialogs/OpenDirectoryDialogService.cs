﻿using System.Windows.Forms;

namespace CIS.UI.Utilities.CommonDialogs;

public class OpenDirectoryDialogService : IOpenDirectoryDialogService
{
    public string Show()
    {
        var dialog = new FolderBrowserDialog() { };
        var result = dialog.ShowDialog();
        return (result == DialogResult.OK) ? dialog.SelectedPath : null;
    }
}
