﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Validations.Barangays
{
    public class IncumbentValidation : ValidationDef<Incumbent>
    {
        public IncumbentValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Officials)
                .NotNullableAndNotEmpty()
                .And.HasValidElements();

            Define(x => x.Date);

            //ValidateInstance.By(instance =>
            //{
            //    instance.Officials.OfType<
            //});
        }
    }
}
