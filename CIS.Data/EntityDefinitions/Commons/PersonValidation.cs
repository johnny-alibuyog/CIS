using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons
{
    public class PersonValidation : ValidationDef<Person>
    {
        public PersonValidation()
        {
            Define(x => x.FirstName)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.MiddleName)
                .MaxLength(150);

            Define(x => x.LastName)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Suffix)
                .MaxLength(150);

            Define(x => x.Gender);

            Define(x => x.BirthDate)
                .IsInThePast();
        }
    }
}
