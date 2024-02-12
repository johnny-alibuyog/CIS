using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class DependentDefinition
{
    public class Mapping : ClassMap<Dependent>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name);

            Map(x => x.Relationship);

            Map(x => x.BirthDate);
        }
    }

    public class Validation : ValidationDef<Dependent>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Relationship);

            Define(x => x.BirthDate);
        }
    }
}
