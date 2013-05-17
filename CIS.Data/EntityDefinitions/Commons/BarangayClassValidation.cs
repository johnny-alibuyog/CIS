using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Validations.Commons
{
    public class BarangayClassValidation : ValidationDef<BarangayClass>
    {
        public BarangayClassValidation()
        {
            Define(x => x.Id)
                .NotNullableAndNotEmpty()
                .And.MaxLength(2);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);
        }
    }
}
