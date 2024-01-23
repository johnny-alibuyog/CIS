using CIS.Core.Entities.Mayors;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Mayors;

public class OfficialValidation : ValidationDef<Official>
{
    public OfficialValidation()
    {
        Define(x => x.Id);

        Define(x => x.Version);

        Define(x => x.Audit);

        Define(x => x.Person)
            .NotNullable()
            .And.IsValid();
    }
}
