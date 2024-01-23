using CIS.Core.Entities.Firearms;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.FireArms;

public class KindMapping : ClassMap<Kind>
{
    public KindMapping()
    {
        Id(x => x.Id);

        Map(x => x.Name)
            .Unique();
    }
}
