﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CIS.UI.Utilities.CommonDialogs
{
    public static class OpenDirectoryDialog
    {
        public static string Show()
        {
            var dialog = new FolderBrowserDialog() {  };
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                return dialog.SelectedPath;
            else
                return null;
        }
    }
}
