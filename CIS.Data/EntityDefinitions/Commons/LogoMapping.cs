using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Commons
{
    public class LogoMapping : ClassMap<Logo>
    {
        public LogoMapping()
        {
            Id(x => x.Id);

            References(x => x.Image)
                .Cascade.All()
                .Fetch.Join();
        }
    }
}
