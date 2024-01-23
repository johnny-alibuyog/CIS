﻿using CIS.Core.Entities.Mayors;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Mayors;

public class OfficeMapping : ClassMap<Office>
{
    public OfficeMapping()
    {
        OptimisticLock.Version();

        Id(x => x.Id);

        Version(x => x.Version);

        Component(x => x.Audit);

        Map(x => x.Name);

        Map(x => x.Location);

        Component(x => x.Address);

        References(x => x.Incumbent)
            .Cascade.SaveUpdate();
    }
}
