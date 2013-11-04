using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Polices
{
    public class ExpiredLicenseHitValidation : ValidationDef<ExpiredLicenseHit>
    {
        public ExpiredLicenseHitValidation()
        {
            Define(x => x.License)
                .NotNullable();

            Define(x => x.ExpiryDate);
        }
    }
}
