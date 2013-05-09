using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Commons
{
    public class LandlineMapping : SubclassMap<Landline>
    {
        public LandlineMapping()
        {
            DiscriminatorValue("Landline");
        }
    }
}
