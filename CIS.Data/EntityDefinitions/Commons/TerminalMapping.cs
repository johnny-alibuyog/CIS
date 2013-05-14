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
            Id(x => x.Id);

            Map(x => x.PcName)
                .Index("PcNameIndex");

            Map(x => x.IpAddress)
                .Index("IpAddressIndex");

            Map(x => x.MacAddress)
                .Index("MacAddressIndex");

            Map(x => x.WithDefaultLogin);

            Map(x => x.WithFingerPrintDevice);

            HasManyToMany(x => x.FingersToScan)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("TerminalsFingersToScan")
                .Cascade.SaveUpdate()
                .AsSet();
        }
    }
}
