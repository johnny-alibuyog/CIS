using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class AuditDefinition
{
    public class Mapping : ComponentMap<Audit>
    {
        public Mapping()
        {
            Map(x => x.CreatedBy);

            Map(x => x.UpdatedBy);

            Map(x => x.CreatedOn);

            Map(x => x.UpdatedOn);
        }
    }

    public class Validation : ValidationDef<Audit>
    {
        public Validation()
        {
            Define(x => x.CreatedBy)
                .MaxLength(50);

            Define(x => x.UpdatedBy)
                .MaxLength(50);

            Define(x => x.CreatedOn);

            Define(x => x.UpdatedOn);
        }
    }
}
