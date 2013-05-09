﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Polices
{
    public class StationMapping : ClassMap<Station>
    {
        public StationMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            References(x => x.Logo)
                .Cascade.All()
                .Fetch.Join();

            Map(x => x.Name);

            Map(x => x.Location);

            Map(x => x.ClearanceValidity);

            Component(x => x.Address);

            HasMany(x => x.Officers)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();
        }
    }
}
