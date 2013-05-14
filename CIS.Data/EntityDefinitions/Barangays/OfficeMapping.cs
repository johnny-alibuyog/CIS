using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Barangays
{
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
}
