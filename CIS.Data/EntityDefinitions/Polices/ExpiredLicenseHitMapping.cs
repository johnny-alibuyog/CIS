using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Polices
{
    public class ExpiredLicenseHitMapping : SubclassMap<ExpiredLicenseHit>
    {
        public ExpiredLicenseHitMapping()
        {
            DiscriminatorValue("ExpiredLicenseHit");

            References(x => x.License);

            Map(x => x.ExpiryDate);
        }
    }
}
