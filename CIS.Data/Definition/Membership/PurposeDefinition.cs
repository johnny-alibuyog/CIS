using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class PurposeDefinition
{
    public class Mapping : ClassMap<Purpose>
    {
        public Mapping()
        {
            Id(x => x.Id);

            Map(x => x.Name);
        }
    }

    public class Validaton : ValidationDef<Purpose>
    {
        public Validaton()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(250);
        }
    }
}
