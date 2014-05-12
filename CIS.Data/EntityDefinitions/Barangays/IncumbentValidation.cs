using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Barangays
{
    public class IncumbentValidation : ValidationDef<Incumbent>
    {
        public IncumbentValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Date);

            Define(x => x.Officials)
                .NotNullableAndNotEmpty()
                .And.HasValidElements();

            //ValidateInstance.By(instance =>
            //{
            //    instance.Officials.OfType<
            //});
        }
    }
}
