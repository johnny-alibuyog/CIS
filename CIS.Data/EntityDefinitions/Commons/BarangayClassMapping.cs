using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Commons
{
    public class BarangayClassMapping : ClassMap<BarangayClass>
    {
        public BarangayClassMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name);
        }
    }
}
