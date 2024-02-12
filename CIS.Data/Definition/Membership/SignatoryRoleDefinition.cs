using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class SignatoryRoleDefinition
{
    public class Mapping : ClassMap<SignatoryRole>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name);
        }
    }

    public class Validation : ValidationDef<SignatoryRole>
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
