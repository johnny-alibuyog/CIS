using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Polices
{
    public class ApplicantMapping : ClassMap<Applicant>
    {
        public ApplicantMapping()
        {
            Id(x => x.Id);

            Component(x => x.Person);

            Component(x => x.Address);

            References(x => x.Picture);

            References(x => x.FingerPrint);

            Map(x => x.Height);

            Map(x => x.Weight);

            Map(x => x.AlsoKnownAs);

            Map(x => x.BirthPlace);

            Map(x => x.Occupation);

            Map(x => x.Religion);

            References(x => x.Purpose);
        }
    }
}
