using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class BlotterHitValidation: ValidationDef<BlotterHit>
    {
        public BlotterHitValidation()
        {
            Define(x => x.Respondent)
                .NotNullable();

            Define(x => x.Blotter)
                .NotNullable();
        }
    }
}
