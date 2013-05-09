using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Commons
{
    public class AuditMapping : ComponentMap<Audit>
    {
        public AuditMapping()
        {
            Map(x => x.CreatedBy);

            Map(x => x.UpdatedBy);

            Map(x => x.CreatedOn);

            Map(x => x.UpdatedOn);
        }
    }
}
