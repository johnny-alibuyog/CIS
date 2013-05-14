using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons
{
    public class FaxValidation : ValidationDef<Fax>
    {
        public FaxValidation()
        {
            Define(x => x.Id);

            Define(x => x.Value);
        }
    }
}
