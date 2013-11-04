using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Polices
{
    public class SuspectHitValidation : ValidationDef<SuspectHit>
    {
        public SuspectHitValidation()
        {
            Define(x => x.Suspect)
                .NotNullable();
        }
    }
}
