using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Validations.Commons
{
    public class LandlineValidation : ValidationDef<Landline>
    {
        public LandlineValidation()
        {
            Define(x => x.Id);

            Define(x => x.Value);
        }
    }
}
