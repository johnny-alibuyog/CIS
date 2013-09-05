using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Features.Commons.Persons;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Memberships.Users
{
    public class UserViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        [NotNullNotEmpty(Message = "Username is mandatory.")]
        public virtual string Username { get; set; }

        [Email(Message = "Invalid email address.")]
        public virtual string Email { get; set; }

        [NotNullNotEmpty(Message = "Password is mandatory.")]
        public virtual string Password { get; set; }

        [NotNullNotEmpty(Message = "Confirm Password is mandatory.")]
        public virtual string ConfirmPassowrd { get; set; }

        [Valid]
        public virtual PersonViewModel Person { get; set; }

        public virtual IReactiveList<UserRoleViewModel> Roles { get; set; }

        public virtual IReactiveCommand Save { get; set; }
    }
}
