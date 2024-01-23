using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Barangays;

public class CommitteeMapping : ClassMap<Committee>
{
    public CommitteeMapping()
    {
        Id(x => x.Id)
            .GeneratedBy.Assigned();

        Map(x => x.Name);

        References(x => x.Position);
    }
}
