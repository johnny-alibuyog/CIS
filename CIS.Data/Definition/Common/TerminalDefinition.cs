using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class TerminalDefinition
{
    public class Mapping : ClassMap<Terminal>
    {
        public Mapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            Map(x => x.MachineName)
                .Index("MachineNameIndex");

            Map(x => x.IpAddress)
                .Index("IpAddressIndex");

            Map(x => x.MacAddress)
                .Index("MacAddressIndex");

            Map(x => x.WithDefaultLogin);
        }
    }

    public class Validation : ValidationDef<Terminal>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.MachineName)
                .MaxLength(100);

            Define(x => x.IpAddress)
                .MaxLength(100);

            Define(x => x.MacAddress)
                .MaxLength(100);

            Define(x => x.WithDefaultLogin);
        }
    }
}
