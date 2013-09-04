using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Memberships.Users
{
    public class UserListItemViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string FullName { get; set; }
    }
}
