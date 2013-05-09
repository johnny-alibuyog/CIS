using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Commons
{
    public class PhysicalAttributesMapping : ComponentMap<PhysicalAttributes>
    {
        public PhysicalAttributesMapping()
        {
            Map(x => x.Hair);

            Map(x => x.Eyes);

            Map(x => x.Complexion);

            Map(x => x.Build);

            Map(x => x.ScarsAndMarks);

            Map(x => x.Race);

            Map(x => x.Nationality);
        }
    }
}
