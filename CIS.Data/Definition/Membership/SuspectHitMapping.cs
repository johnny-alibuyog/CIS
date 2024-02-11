using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;

namespace CIS.Data.Definition.Membership
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
