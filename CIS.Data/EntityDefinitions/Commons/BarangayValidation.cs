using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons
{
    public class BarangayValidation : ValidationDef<Barangay>
    {
        public BarangayValidation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(100);

            Define(x => x.City)
                .NotNullable()
                .And.IsValid();

            Define(x => x.AreaClass);

            Define(x => x.Population);
        }
    }
}
