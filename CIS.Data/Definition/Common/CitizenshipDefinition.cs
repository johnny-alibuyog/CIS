using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class CitizenshipDefinition
{
    public class Mapping : ClassMap<Citizenship>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Descrition);
        }
    }

    public class Validation : ValidationDef<Citizenship>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Descrition)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);
        }
    }
}
