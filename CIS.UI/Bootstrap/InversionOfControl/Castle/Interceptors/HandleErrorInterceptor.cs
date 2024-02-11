using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using CIS.UI.Utilities.CommonDialogs;

namespace CIS.UI.Bootstraps.InversionOfControl.Castle.Interceptors
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
                _messageBox.Warn(ex.Message, ex);
            }
        }
    }
}
