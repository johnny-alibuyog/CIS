using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class ReligionDefinition
{
    public class Mapping : ClassMap<Religion>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name);
        }
    }

    public class Validation : ValidationDef<Religion>
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
