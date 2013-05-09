using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Memberships;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Memberships
{
    public class RoleMapping : ClassMap<Role>
    {
        public RoleMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name);
        }
    }
}
