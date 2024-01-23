using CIS.Core.Entities.Polices;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Polices;

public class HitValidation : ValidationDef<Hit>
{
    public HitValidation()
    {
        Define(x => x.Id);

        Define(x => x.Finding)
            .NotNullable();

        Define(x => x.HitScore);

        Define(x => x.IsIdentical);
    }
}
