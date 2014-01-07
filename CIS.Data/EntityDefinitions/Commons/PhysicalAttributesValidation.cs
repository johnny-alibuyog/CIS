using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons
{
    public class PhysicalAttributesValidation : ValidationDef<PhysicalAttributes>
    {
        public PhysicalAttributesValidation()
        {
            Define(x => x.Hair)
                .MaxLength(250);

            Define(x => x.Eyes)
                .MaxLength(250);

            Define(x => x.Complexion)
                .MaxLength(250);

            Define(x => x.Build)
                .MaxLength(250);

            Define(x => x.ScarsAndMarks)
                .MaxLength(250);

            Define(x => x.Race)
                .MaxLength(250);

            Define(x => x.Nationality)
                .MaxLength(250);
        }
    }
}
