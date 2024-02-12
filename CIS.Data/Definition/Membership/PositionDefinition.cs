using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class PositionDefinition
{
    public class Mapping : ClassMap<Position>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name);
        }
    }

    public class Validation : ValidationDef<Position>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);
        }
    }
}
