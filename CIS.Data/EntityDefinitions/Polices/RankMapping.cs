using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Polices;

public class RankMapping : ClassMap<Rank>
{
    public RankMapping()
    {
        Id(x => x.Id)
            .GeneratedBy.Assigned();

        Map(x => x.Name);

        Map(x => x.Category);
    }
}
