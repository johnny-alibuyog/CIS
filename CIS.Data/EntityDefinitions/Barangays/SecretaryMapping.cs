using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Barangays
{
    public class SecretaryMapping : SubclassMap<Secretary>
    {
        public SecretaryMapping()
        {
            DiscriminatorValue("Secretary");
        }
    }
}
