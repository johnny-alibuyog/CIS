using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Firearms;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.FireArms
{
    public class KindValidation : ValidationDef<Kind>
    {
        public KindValidation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(250);
        }
    }
}
