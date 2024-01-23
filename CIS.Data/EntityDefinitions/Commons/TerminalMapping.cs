using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons;

public class TerminalMapping : ClassMap<Terminal>
{
    public TerminalMapping()
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
