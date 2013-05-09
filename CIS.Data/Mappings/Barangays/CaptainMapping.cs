using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Barangays
{
    public class CaptainMapping : SubclassMap<Captain>
    {
        public CaptainMapping()
        {
            DiscriminatorValue("Captain");
        }
    }
}
