using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Polices;
using CIS.Data.Commons.Extentions;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Polices
{
    public class SuspectMapping : ClassMap<Suspect>
    {
        public SuspectMapping()
        {
            //OptimisticLock.Version();

            Id(x => x.Id);

            //Version(x => x.Version);

            Component(x => x.Audit);

            References(x => x.Warrant);

            Map(x => x.ArrestStatus);

            Component(x => x.Person);

            Component(x => x.Address);

            Component(x => x.PhysicalAttributes);

            HasMany(x => x.Aliases)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Cascade.AllDeleteOrphan()
                .Fetch.Subselect()
                .Table("SuspectAliases")
                .Element("Name");

            HasMany(x => x.Occupations)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Cascade.AllDeleteOrphan()
                .Fetch.Subselect()
                .Table("SuspectOccupations")
                .Element("Value");
        }
    }
}
