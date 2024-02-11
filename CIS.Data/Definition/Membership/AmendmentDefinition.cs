using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class AmendmentDefinition
{
    public class Mapping : ClassMap<Amendment>
    {
        public Mapping()
        {
            Id(x => x.Id);

            References(x => x.Approver);

            Map(x => x.DocumentNumber);

            Map(x => x.Reason);

            Map(x => x.Remarks);
        }
    }

    public class Validation : ValidationDef<Amendment>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.DocumentNumber)
                .MaxLength(50);

            Define(x => x.Reason)
                .MaxLength(250);

            Define(x => x.Remarks)
                .MaxLength(250);
        }
    }
}
