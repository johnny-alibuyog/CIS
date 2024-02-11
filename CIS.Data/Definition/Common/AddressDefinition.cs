using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class AddressDefinition
{
    public class Mapping : ComponentMap<Address>
    {
        public Mapping()
        {
            Map(x => x.Address1);

            Map(x => x.Address2);

            Map(x => x.Barangay);

            Map(x => x.City);

            Map(x => x.Province);
        }
    }

    public class Validation : ValidationDef<Address>
    {
        public Validation()
        {
            Define(x => x.Address1)
                .MaxLength(200);

            Define(x => x.Address2)
                .MaxLength(200);

            Define(x => x.Barangay)
                .MaxLength(100);

            Define(x => x.City)
                .MaxLength(100);

            Define(x => x.Province)
                .MaxLength(100);
        }
    }
}
