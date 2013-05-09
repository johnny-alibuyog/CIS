using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Mayors;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Mayors
{
    public class SecretaryMapping : SubclassMap<Secretary>
    {
        public SecretaryMapping()
        {
            DiscriminatorValue("Secretary");
        }
    }
}
