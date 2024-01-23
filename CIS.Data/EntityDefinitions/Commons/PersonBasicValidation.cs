using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Commons;

public class PersonBasicValidation : ValidationDef<PersonBasic>
{
    public PersonBasicValidation()
    {
        Define(x => x.Prefix)
            .MaxLength(150);

        Define(x => x.FirstName)
            .MaxLength(150);

        Define(x => x.MiddleName)
            .MaxLength(150);

        Define(x => x.LastName)
            .MaxLength(150);

        Define(x => x.Suffix)
            .MaxLength(150);
    }
}
