﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Utilities.CommonDialogs
{
    public interface IMessageBoxService
    {
        void Warn(string message, string caption = "Warning");
        void Warn(string message, Exception ex, string caption = "Warning");
        bool Confirm(string message, string caption = "Confirmation");
        void Inform(string message, string caption = "Information");
    }
}
