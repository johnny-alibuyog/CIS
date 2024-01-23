using CIS.Core.Entities.Polices;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Polices;

public class PurposeValidaton : ValidationDef<Purpose>
{
    public PurposeValidaton()
    {
        Define(x => x.Id);

        Define(x => x.Name)
            .NotNullableAndNotEmpty()
            .And.MaxLength(250);
    }
}
