using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons;

public class ContactMapping: ClassMap<Contact>
{
    public ContactMapping()
    {
        Id(x => x.Id);

        Map(x => x.Value);

        DiscriminateSubClassesOnColumn("Discriminator")
            .Index("DiscriminatorIndex")
            .Not.Nullable()
            .Length(25);
    }
}
