using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Validations.Barangays
{
    public class PositionValidation : ValidationDef<Position>
    {
        public PositionValidation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(100);
        }
    }
}
