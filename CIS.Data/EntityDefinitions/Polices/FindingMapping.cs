using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Polices;

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
