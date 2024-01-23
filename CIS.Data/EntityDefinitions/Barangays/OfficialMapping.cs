using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Barangays;

public class OfficialMapping : ClassMap<Official>
{
    public OfficialMapping()
    {
        OptimisticLock.Version();

        Id(x => x.Id);

        Version(x => x.Version);

        Component(x => x.Audit);

        Component(x => x.Person);

        References(x => x.Position)
            .Fetch.Join();

        References(x => x.Committee)
            .Fetch.Join();

        References(x => x.Incumbent);

        References(x => x.Picture)
            .Cascade.All()
            .Fetch.Join();

        References(x => x.Signature)
            .Cascade.All()
            .Fetch.Join();

        Map(x => x.IsActive);
    }
}
