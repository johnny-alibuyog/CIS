using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons;

public class BlobMapping : ClassMap<Blob>
{
    public BlobMapping()
    {
        Id(x => x.Id);

        Map(x => x.Bytes);

        DiscriminateSubClassesOnColumn("Discriminator")
            .Index("DiscriminatorIndex")
            .Not.Nullable()
            .Length(25);
    }
}
