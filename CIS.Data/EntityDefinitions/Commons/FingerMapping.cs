using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons;

public class FingerMapping : ClassMap<Finger>
{
    public FingerMapping()
    {
        Id(x => x.Id)
            .GeneratedBy.Assigned();

        Map(x => x.Name);

        Map(x => x.ImageUri);
    }
}
