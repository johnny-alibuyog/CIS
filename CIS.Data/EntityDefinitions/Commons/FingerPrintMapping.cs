using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons;

public class FingerPrintMapping : ClassMap<FingerPrint>
{
    public FingerPrintMapping()
    {
        OptimisticLock.Version();

        Id(x => x.Id);

        References(x => x.RightThumb)
            .Cascade.All()
            .Fetch.Join();

        References(x => x.RightIndex)
            .Cascade.All()
            .Fetch.Join();

        References(x => x.RightMiddle)
            .Cascade.All()
            .Fetch.Join();

        References(x => x.RightRing)
            .Cascade.All()
            .Fetch.Join();

        References(x => x.RightPinky)
            .Cascade.All()
            .Fetch.Join();

        References(x => x.LeftThumb)
            .Cascade.All()
            .Fetch.Join();

        References(x => x.LeftIndex)
            .Cascade.All()
            .Fetch.Join();

        References(x => x.LeftMiddle)
            .Cascade.All()
            .Fetch.Join();

        References(x => x.LeftRing)
            .Cascade.All()
            .Fetch.Join();

        References(x => x.LeftPinky)
            .Cascade.All()
            .Fetch.Join();
    }
}
