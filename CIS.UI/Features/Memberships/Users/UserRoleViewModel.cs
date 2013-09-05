using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Memberships.Users
{
    public class UserRoleViewModel : ViewModelBase
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual bool Checked { get; set; }
    }
}
