using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Barangays;

public class PurposeValidation : ValidationDef<Purpose>
{
    public PurposeValidation()
    {
        Define(x => x.Id);

        Define(x => x.Name)
            .NotNullableAndNotEmpty()
            .And.MaxLength(250);
    }
}
