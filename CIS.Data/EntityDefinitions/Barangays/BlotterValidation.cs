using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class BlotterValidation : ValidationDef<Blotter>
    {
        public BlotterValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Complaints)
                .NotNullableAndNotEmpty()
                .And.HasValidElements();

            Define(x => x.Complaints)
                .NotNullableAndNotEmpty()
                .And.HasValidElements();

            Define(x => x.Defendants)
                .NotNullableAndNotEmpty()
                .And.HasValidElements();
        }
    }
}
