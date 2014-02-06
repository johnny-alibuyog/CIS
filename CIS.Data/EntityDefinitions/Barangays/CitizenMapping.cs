using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class CitizenMapping : ClassMap<Citizen>
    {
        public CitizenMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            Component(x => x.Person);

            Map(x => x.CivilStatus);

            Map(x => x.AlsoKnownAs);

            Map(x => x.BirthPlace);

            Map(x => x.Occupation);

            Map(x => x.Religion);

            Map(x => x.Citizenship);

            Map(x => x.EmailAddress);

            Map(x => x.TelephoneNumber);

            Map(x => x.CellphoneNumber);

            Component(x => x.CurrentAddress);

            Component(x => x.ProvincialAddress);
        }
    }
}
