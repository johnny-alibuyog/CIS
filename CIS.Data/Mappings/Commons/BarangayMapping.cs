using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Commons
{
    public class BarangayMapping : ClassMap<Barangay>
    {
        public BarangayMapping()
        {
            Id(x => x.Id);

            Map(x => x.Name);

            References(x => x.City);

            Map(x => x.AreaClass);

            Map(x => x.Population);
        }
    }
}
