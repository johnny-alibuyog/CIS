using CIS.Core.Entities.Polices;
using CIS.Data.Commons.Extentions;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Polices;

public class SuspectMapping : ClassMap<Suspect>
{
    public SuspectMapping()
    {
        //OptimisticLock.Version();

        Id(x => x.Id);

        Map(x => x.DataStoreId)
            .Index("DataStoreIdIndex");

        Map(x => x.DataStoreChildKey)
            .Index("DataStoreChildKey");

        //Version(x => x.Version);

        Component(x => x.Audit);

        References(x => x.Warrant);

        Map(x => x.ArrestStatus);

        Map(x => x.ArrestDate);

        Map(x => x.Disposition);

        Component(x => x.Person);

        Component(x => x.Address);

        Component(x => x.PhysicalAttributes);

        HasMany(x => x.Aliases)
            .Access.CamelCaseField(Prefix.Underscore)
            .Schema(GetType().ParseSchema())
            .Cascade.AllDeleteOrphan()
            .Fetch.Subselect()
            .Table("SuspectAliases")
            .Element("Name")
            .AsSet();

        HasMany(x => x.Occupations)
            .Access.CamelCaseField(Prefix.Underscore)
            .Schema(GetType().ParseSchema())
            .Cascade.AllDeleteOrphan()
            .Fetch.Subselect()
            .Table("SuspectOccupations")
            .Element("Value")
            .AsSet();
    }
}
