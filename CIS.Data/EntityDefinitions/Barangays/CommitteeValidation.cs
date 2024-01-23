using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Barangays;

public class CommitteeValidation : ValidationDef<Committee>
{
    public CommitteeValidation()
    {
        Define(x => x.Id)
            .NotNullableAndNotEmpty()
            .And.MaxLength(10);

        Define(x => x.Name)
            .NotNullableAndNotEmpty()
            .And.MaxLength(150);

        Define(x => x.Position)
            .NotNullable()
            .And.IsValid();
    }
}
