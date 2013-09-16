using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Memberships;
using CIS.UI.Utilities.CommonDialogs;
using Ninject.Extensions.Interception;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors
{
    public class AuthorizeInterceptor : IInterceptor
    {
        private readonly Role[] _roles;
        private readonly IMessageBoxService _messageBox;

        public AuthorizeInterceptor(Role[] roles, IMessageBoxService messageBox)
        {
            _roles = roles;
            _messageBox = messageBox;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                if (App.Data.User == null)
                {
                    _messageBox.Warn("No user is currently logged-in.");
                    return;
                }

                if (App.Data.User.IsAuthorized(_roles) != true)
                {
                    _messageBox.Warn(string.Format("User is not authorized. (Authorized Roles: {0})", string.Join<Role>(", ", _roles)));
                    return;
                }

                invocation.Proceed();
            }
            catch (Exception ex)
            {
                _messageBox.Warn(ex.Message, ex);
            }
        }
    }
}
