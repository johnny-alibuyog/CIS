using CIS.Core.Domain.Security;
using CIS.Core.Utility.Extention;

namespace CIS.UI.Features.Security.Users
{
    public class UserRoleViewModel : ViewModelBase
    {
        public virtual bool IsChecked { get; set; }

        public virtual Role Role { get; set; }

        public virtual string Description { get { return this.Role.GetDescription(); } }

        public UserRoleViewModel() { }

        public UserRoleViewModel(bool isChecked, Role role)
        {
            this.IsChecked = isChecked;
            this.Role = role;
        }


    }
}
