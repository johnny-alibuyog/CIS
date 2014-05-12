using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class BlotterHitMapping  : SubclassMap<BlotterHit>
    {
        public BlotterHitMapping()
        {
            DiscriminatorValue("BlotterHit");

            References(x => x.Respondent);

            References(x => x.Blotter);
        }
    }
}
