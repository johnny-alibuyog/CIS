using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons;

public class AddressMapping : ComponentMap<Address>
{
    public AddressMapping()
    {
        Map(x => x.Address1);

        Map(x => x.Address2);

        Map(x => x.Barangay);

        Map(x => x.City);

        Map(x => x.Province);
    }
}
