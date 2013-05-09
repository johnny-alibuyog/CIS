using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Validations.Barangays
{
    public class OfficialValidation : ValidationDef<Official>
    {
        public OfficialValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();
        }
    }
}
