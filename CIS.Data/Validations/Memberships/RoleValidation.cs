using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Memberships;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Validations.Memberships
{
    public class RoleValidation : ValidationDef<Role>
    {
        public RoleValidation()
        {
            Define(x => x.Id)
                .NotNullableAndNotEmpty()
                .And.MaxLength(2);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(100);
        }
    }
}
