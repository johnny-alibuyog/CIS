using CIS.Core.Entities.Firearms;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.FireArms;

public class MakeValidation : ValidationDef<Make>
{
    public MakeValidation()
    {
        Define(x => x.Id);

        Define(x => x.Name);
    }
}
