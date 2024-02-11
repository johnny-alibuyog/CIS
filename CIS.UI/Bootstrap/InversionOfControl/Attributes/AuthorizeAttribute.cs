using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Memberships;

namespace CIS.UI.Bootstraps.InversionOfControl.Attributes
{
    public class AuthorizeAttribute : Attribute
    {
        public virtual Role[] Roles { get; set; }
    }
}
