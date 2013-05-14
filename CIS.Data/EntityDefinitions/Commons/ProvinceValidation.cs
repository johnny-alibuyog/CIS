using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons
{
    public class ProvinceValidation : ValidationDef<Province>
    {
        public ProvinceValidation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(100);

            Define(x => x.Region)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Cities)
                .HasValidElements();
        }
    }
}
