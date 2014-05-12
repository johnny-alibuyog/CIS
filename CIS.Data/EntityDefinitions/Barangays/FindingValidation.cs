using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class FindingValidation : ValidationDef<Finding>
    {
        public FindingValidation()
        {
            Define(x => x.Id);

            Define(x => x.FinalFindings)
                .MaxLength(500);

            Define(x => x.Amendment);

            Define(x => x.Hits)
                .HasValidElements();
        }
    }
}
