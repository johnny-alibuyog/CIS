using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class RankDefinition
{
    public class Mapping : ClassMap<Rank>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name);

            Map(x => x.Category);
        }
    }

    public class Validation : ValidationDef<Rank>
    {
        public Validation()
        {
            Define(x => x.Id)
                .NotNullableAndNotEmpty()
                .And.MaxLength(10);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(100);

            Define(x => x.Category);
        }
    }
}
