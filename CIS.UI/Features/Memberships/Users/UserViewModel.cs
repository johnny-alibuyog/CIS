using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Memberships;
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

        public virtual string[] GetSelctedRoleIds()
        {
            return this.Roles.Where(x => x.Checked).Select(x => x.Id).ToArray();
        }

        public UserViewModel()
        {
            this.WhenAny(x => x.Person.IsValid, x => true)
                .Subscribe(x => this.Revalidate());
        }

        public override object SerializeInto(object instance)
        {
            if (instance == null)
                return null;

            if (instance is UserViewModel)
            {
                var source = this;
                var target = instance as UserViewModel;

                target.SerializeWith(source);
                return target;
            }
            else if (instance is User)
            {
                var source = this;
                var target = instance as User;

                target.Username = source.Username;
                target.Password = source.Password;
                target.Email = source.Email;
                target.Person = (Person)source.SerializeInto(new Person());
                //target.Roles = Role.GetByIds(source.GetSelctedRoleIds()); // Todo: fix

                return target;
            }

            return null;
        }

        public override object SerializeWith(object instance)
        {
            return base.SerializeWith(instance);
        }


    }
}
