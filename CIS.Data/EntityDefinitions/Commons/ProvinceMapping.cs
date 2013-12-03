using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons
{
    public class ProvinceMapping : ClassMap<Province>
    {
        public ProvinceMapping()
        {
            Id(x => x.Id);

            Map(x => x.Name);

            References(x => x.Region);

            HasMany(x => x.Cities)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsBag();
        }
    }
}
