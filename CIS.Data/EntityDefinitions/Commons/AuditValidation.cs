using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons;

public class AuditValidation : ValidationDef<Audit>
{
    public AuditValidation()
    {
        Define(x => x.CreatedBy)
            .MaxLength(50);

        Define(x => x.UpdatedBy)
            .MaxLength(50);

        Define(x => x.CreatedOn);

        Define(x => x.UpdatedOn);
    }
}
