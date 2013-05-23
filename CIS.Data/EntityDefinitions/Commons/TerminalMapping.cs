using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Data.Commons.Extentions;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons
{
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
}
