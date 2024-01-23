using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Polices;

public class PurposeMapping : ClassMap<Purpose>
{
    public PurposeMapping()
    {
        Id(x => x.Id);

        Map(x => x.Name);
    }
}
