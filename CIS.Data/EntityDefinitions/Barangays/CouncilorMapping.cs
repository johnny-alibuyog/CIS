using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Barangays
{
    public class CouncilorMapping : SubclassMap<Councilor>
    {
        public CouncilorMapping()
        {
            DiscriminatorValue("Councilor");
        }
    }
}
