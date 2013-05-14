using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Barangays
{
    public class KagawadMapping: SubclassMap<Kagawad>
    {
        public KagawadMapping()
        {
            DiscriminatorValue("Kagawad");
        }
    }
}
