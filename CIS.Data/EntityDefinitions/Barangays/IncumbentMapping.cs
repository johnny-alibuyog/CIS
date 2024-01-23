﻿using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Barangays;

public class IncumbentMapping : ClassMap<Incumbent>
{
    public IncumbentMapping()
    {
        OptimisticLock.Version();

        Id(x => x.Id);

        Version(x => x.Version);

        Component(x => x.Audit);

        Map(x => x.Date);

        HasMany(x => x.Officials)
            .Access.CamelCaseField(Prefix.Underscore)
            .Cascade.AllDeleteOrphan()
            .Not.KeyNullable()
            .Not.KeyUpdate()
            .Inverse()
            .AsSet();
    }
}
