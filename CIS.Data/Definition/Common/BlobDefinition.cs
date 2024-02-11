using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class BlobDefinition
{
    public class Mapping : ClassMap<Blob>
    {
        public Mapping()
        {
            Id(x => x.Id);

            Map(x => x.Bytes);

            DiscriminateSubClassesOnColumn("Discriminator")
                .Index("DiscriminatorIndex")
                .Not.Nullable()
                .Length(25);
        }
    }

    public class Validation : ValidationDef<Blob>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Bytes);
        }
    }
}
