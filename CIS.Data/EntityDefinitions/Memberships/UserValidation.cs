using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Memberships;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Memberships
{
    public class UserValidation : ValidationDef<User>
    {
        public UserValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Username)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);

            Define(x => x.Password)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);

            Define(x => x.Email)
                .NotNullableAndNotEmpty()
                .And.IsEmail();

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Roles)
                .NotNullableAndNotEmpty();
                //.And.HasValidElements();
        }
    }
}
