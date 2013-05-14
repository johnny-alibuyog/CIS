using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons
{
    public class BodyStatisticsMapping : ComponentMap<BodyStatistics>
    {
        public BodyStatisticsMapping()
        {
            Map(x => x.Height);

            Map(x => x.Weight);
        }
    }
}
