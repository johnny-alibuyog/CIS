using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class HitValidation : ValidationDef<Hit>
    {
        public HitValidation()
        {
            Define(x => x.Id);

            Define(x => x.Finding)
                .NotNullable();

            Define(x => x.HitScore);

            Define(x => x.IsIdentical);
        }
    }
}
