using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons;

public class FingerValidation : ValidationDef<Finger>
{
    public FingerValidation()
    {
        Define(x => x.Id)
            .NotNullableAndNotEmpty()
            .And.MaxLength(2);

        Define(x => x.Name)
            .NotNullableAndNotEmpty()
            .And.MaxLength(25);

        Define(x => x.ImageUri)
            .NotNullableAndNotEmpty()
            .And.MaxLength(300);
    }
}
