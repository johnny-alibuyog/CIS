using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class HitDefinition
{
    public class Mapping : ClassMap<Hit>
    {
        public Mapping()
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

    public class Validation : ValidationDef<Hit>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Finding)
                .NotNullable();

            Define(x => x.HitScore);

            Define(x => x.IsIdentical);
        }
    }
}
