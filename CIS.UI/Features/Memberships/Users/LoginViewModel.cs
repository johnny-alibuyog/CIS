using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Memberships.Users
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly LoginController _controller;

        [NotNullNotEmpty(Message  = "Username is mandatory")]
        public virtual string Username { get; set; }

        [NotNullNotEmpty(Message = "Password is mandatory")]
        public virtual string Password { get; set; }

        public virtual IReactiveCommand Login { get; set; }

        public LoginViewModel()
        {
            _controller = IoC.Container.Resolve<LoginController>(new ViewModelDependency(this));
        }
    }
}
