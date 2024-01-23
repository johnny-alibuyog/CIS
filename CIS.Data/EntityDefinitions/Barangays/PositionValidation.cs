using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Validations.Barangays;

public class PositionValidation : ValidationDef<Position>
{
    public PositionValidation()
    {
        Define(x => x.Id)
            .NotNullableAndNotEmpty()
            .And.MaxLength(10);

        Define(x => x.Name)
            .NotNullableAndNotEmpty()
            .And.MaxLength(100);

        Define(x => x.Committees)
            .HasValidElements();
    }
}
