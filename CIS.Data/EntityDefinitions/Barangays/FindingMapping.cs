using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class FindingMapping : ClassMap<Finding>
    {
        public FindingMapping()
        {
            Id(x => x.Id);

            Map(x => x.FinalFindings);

            References(x => x.Amendment)
                .Cascade.All();

            HasMany(x => x.Hits)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();
        }
    }
}
