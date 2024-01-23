using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Barangays;

public class PositionMapping : ClassMap<Position>
{
    public PositionMapping()
    {
        Id(x => x.Id)
            .GeneratedBy.Assigned();

        Map(x => x.Name);

        HasMany(x => x.Committees)
            .Access.CamelCaseField(Prefix.Underscore)
            .Cascade.AllDeleteOrphan()
            .Not.KeyNullable()
            .Not.KeyUpdate()
            .Inverse()
            .AsSet()
            .Cache.ReadWrite();
    }
}
