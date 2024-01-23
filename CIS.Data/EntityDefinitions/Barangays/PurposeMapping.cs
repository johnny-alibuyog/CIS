using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Barangays;

public class PurposeMapping : ClassMap<Purpose>
{
    public PurposeMapping()
    {
        Id(x => x.Id);

        Map(x => x.Name);
    }
}
