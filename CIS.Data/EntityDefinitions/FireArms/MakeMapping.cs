using CIS.Core.Entities.Firearms;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.FireArms;

public class MakeMapping : ClassMap<Make>
{
    public MakeMapping()
    {
        Id(x => x.Id);

        Map(x => x.Name)
            .Unique();
    }
}
