using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Mayors;
using CIS.Data.Commons.Extentions;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Mayors
{
    public class IncumbentMapping : ClassMap<Incumbent>
    {
        public IncumbentMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            HasManyToMany(x => x.Officials)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("IncumbentsOfficials")
                .Cascade.AllDeleteOrphan()
                .AsSet();

            Map(x => x.Date);
        }
    }
}
