using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Validations.Commons
{
    public class CityValidation : ValidationDef<City>
    {
        public CityValidation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(100);

            Define(x => x.Province)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Barangays)
                .HasValidElements();
        }
    }
}
