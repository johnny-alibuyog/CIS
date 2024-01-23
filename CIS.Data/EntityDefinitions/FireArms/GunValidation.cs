using CIS.Core.Entities.Firearms;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.FireArms;

public class GunValidation : ValidationDef<Gun>
{
    public GunValidation()
    {
        Define(x => x.Model)
            .MaxLength(250);

        Define(x => x.Caliber)
            .MaxLength(250);

        Define(x => x.SerialNumber)
            .MaxLength(50);

        Define(x => x.Kind);

        Define(x => x.Make);
    }
}
