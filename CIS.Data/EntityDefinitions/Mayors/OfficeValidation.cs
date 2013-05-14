using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Mayors;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Mayors
{
    public class OfficeValidation : ValidationDef<Office>
    {
        public OfficeValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(250);

            Define(x => x.Location)
                .NotNullableAndNotEmpty()
                .And.MaxLength(250);

            Define(x => x.Address)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Incumbent)
                .NotNullable()
                .And.IsValid();
        }
    }
}
