using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Polices
{
    public class SuspectHitMapping : SubclassMap<SuspectHit>
    {
        public SuspectHitMapping()
        {
            DiscriminatorValue("SuspectHit");

            References(x => x.Suspect);
        }
    }
}
