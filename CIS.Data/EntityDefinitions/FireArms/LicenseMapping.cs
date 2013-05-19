using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Firearms;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.FireArms
{
    public class LicenseMapping : ClassMap<License>
    {
        public LicenseMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            Component(x => x.Person);

            Component(x => x.Address);

            Component(x => x.Gun);

            Map(x => x.LicenseNumber);

            Map(x => x.ControlNumber);

            Map(x => x.IssueDate);

            Map(x => x.ExpiryDate);
        }
    }
}
