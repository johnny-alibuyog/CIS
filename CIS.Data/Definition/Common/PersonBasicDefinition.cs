using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class PersonBasicDefinition
{
    public class Mapping : ComponentMap<PersonBasic>
    {
        public Mapping()
        {
            Map(x => x.Prefix);

            Map(x => x.FirstName)
                .Index("FirstNameIndex");

            Map(x => x.MiddleName)
                .Index("MiddleNameIndex");

            Map(x => x.LastName)
                .Index("LastNameIndex");

            Map(x => x.Suffix);
        }
    }

    public class Validation : ValidationDef<PersonBasic>
    {
        public Validation()
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
}
