using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Memberships;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Utilities.Extentions;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using System;
using System.Linq;

namespace CIS.UI.Features.Memberships.Users.MasterList;

public class UserViewModel : ViewModelBase
{
    public virtual Guid Id { get; set; }

    [NotNullNotEmpty(Message = "Username is mandatory.")]
    public virtual string Username { get; set; }

    [Email(Message = "Invalid email address.")]
    [NotNullNotEmpty(Message = "Email is required.")]
    public virtual string Email { get; set; }

    [NotNullNotEmpty(Message = "Password is mandatory.")]
    public virtual string Password { get; set; }

    [NotNullNotEmpty(Message = "Confirm Password is mandatory.")]
    public virtual string ConfirmPassword { get; set; }

    [Valid]
    public virtual PersonViewModel Person { get; set; }

    [NotNullNotEmpty(Message = "Role is mandatory.")]
    public virtual IReactiveList<UserRoleViewModel> Roles { get; set; }

    public virtual IReactiveCommand Save { get; set; }

    public UserViewModel()
    {
        this.Person = new PersonViewModel();

        this.WhenAnyValue(x => x.Person.IsValid)
            .Subscribe(x => this.Revalidate());
    }

    public override object DeserializeInto(object instance)
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
            target.Person = (Person)source.Person.DeserializeInto(new Person());
            target.Roles = source.Roles.Where(x => x.IsChecked).Select(x => x.Role);
            return target;
        }

        return null;
    }

    public override object SerializeWith(object instance)
    {
        if (instance == null)
            return null;

        if (instance is UserViewModel)
        {
            var source = instance as UserViewModel;
            var target = this;

            target.Id = source.Id;
            target.Username = source.Username;
            target.Password = source.Password;
            target.ConfirmPassword = source.ConfirmPassword;
            target.Email = source.Email;
            target.Person.SerializeWith(source.Person);
            target.Roles = source.Roles.ToReactiveList(); // new instance of reactive lists
            return target;
        }
        else if (instance is User)
        {
            var source = instance as User;
            var target = this;

            target.Id = source.Id;
            target.Username = source.Username;
            target.Password = source.Password;
            target.ConfirmPassword = source.Password;
            target.Email = source.Email;
            target.Person.SerializeWith(source.Person);
            target.Roles = Enum.GetValues(typeof(Role)).Cast<Role>()
                .Select(x => new UserRoleViewModel(source.Roles.Contains(x), x))
                .ToReactiveList(); ;
            return target;
        }
        return null;
    }
}
