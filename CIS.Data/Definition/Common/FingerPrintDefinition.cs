using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class FingerPrintDefinition
{
    public class Mapping : ClassMap<FingerPrint>
    {
        public Mapping()
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

    public class Validation : ValidationDef<FingerPrint>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.RightThumb);

            Define(x => x.RightIndex);

            Define(x => x.RightMiddle);

            Define(x => x.RightRing);

            Define(x => x.RightPinky);

            Define(x => x.LeftThumb);

            Define(x => x.LeftIndex);

            Define(x => x.LeftMiddle);

            Define(x => x.LeftRing);

            Define(x => x.LeftPinky);
        }
    }
}
