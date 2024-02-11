using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class ContactDefinition
{
    public class Mapping : ClassMap<Contact>
    {
        public Mapping()
        {
            Id(x => x.Id);

            Map(x => x.Value);

            DiscriminateSubClassesOnColumn("Discriminator")
                .Index("DiscriminatorIndex")
                .Not.Nullable()
                .Length(25);
        }
    }

    public class Validaton : ValidationDef<Contact>
    {
        public Validaton()
        {
            Define(x => x.Id);

            Define(x => x.Value)
                .NotNullableAndNotEmpty()
                .And.MaxLength(25);
        }
    }
}
