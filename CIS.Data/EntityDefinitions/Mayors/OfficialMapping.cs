using CIS.Core.Entities.Mayors;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Mayors;

public class OfficialMapping : ClassMap<Official>
{
    public OfficialMapping()
    {
        OptimisticLock.Version();

        Id(x => x.Id);

        Version(x => x.Version);

        Component(x => x.Audit);

        Component(x => x.Person);

        DiscriminateSubClassesOnColumn("Discriminator")
            .Index("DiscriminatorIndex")
            .Not.Nullable()
            .Length(25);
    }
}
