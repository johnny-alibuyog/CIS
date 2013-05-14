using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons
{
    public class ContactValidaton : ValidationDef<Contact>
    {
        public ContactValidaton()
        {
            Define(x => x.Id);

            Define(x => x.Value)
                .NotNullableAndNotEmpty()
                .And.MaxLength(25);
        }
    }
}
