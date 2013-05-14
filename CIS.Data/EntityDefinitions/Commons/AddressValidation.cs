using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons
{
    public class AddressValidation : ValidationDef<Address>
    {
        public AddressValidation()
        {
            Define(x => x.Address1)
                .MaxLength(200);

            Define(x => x.Address2)
                .MaxLength(200);

            Define(x => x.Barangay)
                .MaxLength(100);

            Define(x => x.City)
                .MaxLength(100);

            Define(x => x.Province)
                .MaxLength(100);
        }
    }
}
