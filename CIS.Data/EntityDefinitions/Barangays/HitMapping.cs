using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Barangays;

public class HitMapping : ClassMap<Hit>
{
    public HitMapping()
    {
        Id(x => x.Id);

        References(x => x.Finding);

        Map(x => x.HitScore);

        Map(x => x.IsIdentical);

        DiscriminateSubClassesOnColumn("Discriminator")
            .Index("DiscriminatorIndex")
            .Not.Nullable()
            .Length(25);
    }
}
