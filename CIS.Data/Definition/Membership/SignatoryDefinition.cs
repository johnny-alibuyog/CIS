using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class SignatoryDefinition
{
    public class Mapping : ClassMap<Signatory>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Member);
            
            References(x => x.Position);

            References(x => x.Role);

            Map(x => x.IsSigned);

            Map(x => x.DateSigned);
        }
    }

    public class Validation : ValidationDef<Signatory>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Member)
                .NotNullable();

            Define(x => x.Position)
                .NotNullable();

            Define(x => x.Role)
                .NotNullable();
        }
    }
}
