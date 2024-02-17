﻿using CIS.Core.Domain.Security;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Security;

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