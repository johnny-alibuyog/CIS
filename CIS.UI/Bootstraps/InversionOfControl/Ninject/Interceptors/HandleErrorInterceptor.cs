﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Features;
using CIS.UI.Utilities.CommonDialogs;
using Common.Logging;
using Ninject.Extensions.Interception;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors
{
    public class HandleErrorInterceptor : IInterceptor
    {
        private readonly IMessageBoxService _messageBox;

        public HandleErrorInterceptor(IMessageBoxService messageBox)
        {
            _messageBox = messageBox;
        }

        public virtual void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                //var x = (ControllerBase<>)invocation.Request.Proxy;
                //if (invocation.Request.Method.DeclaringType == typeof(ControllerBase<>))
                //{ 
                //}
                //else
                _messageBox.Warn(ex.Message, ex);
            }
        }
    }
}
