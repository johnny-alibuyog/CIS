using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons;

public class FingerPrintValidation : ValidationDef<FingerPrint>
{
    public FingerPrintValidation()
    {
        Define(x => x.Id);

        Define(x => x.RightThumb);

        Define(x => x.RightIndex);

        Define(x => x.RightMiddle);

        Define(x => x.RightRing);

        Define(x => x.RightPinky);

        Define(x => x.LeftThumb);

        Define(x => x.LeftIndex);

        Define(x => x.LeftMiddle);

        Define(x => x.LeftRing);

        Define(x => x.LeftPinky);
    }
}
