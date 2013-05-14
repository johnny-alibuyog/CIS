using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Polices
{
    public class OfficerMapping : ClassMap<Officer>
    {
        public OfficerMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            Component(x => x.Person);

            References(x => x.Station);

            References(x => x.Rank);

            Map(x => x.Position);

            Map(x => x.Title);
        }
    }
}
