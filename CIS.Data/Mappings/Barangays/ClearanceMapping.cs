using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using CIS.Data.Commons.Extentions;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Barangays
{
    public class ClearanceMapping : ClassMap<Clearance>
    {
        public ClearanceMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            Component(x => x.Applicant);

            Component(x => x.Address);

            HasManyToMany(x => x.Officials)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("ClearancesOfficials")
                .Cascade.SaveUpdate()
                .AsSet();

            Map(x => x.Purpose);

            Map(x => x.ClearanceFee);

            Map(x => x.CommunityTaxCertificateNumber);

            Map(x => x.OfficialReceiptNumber);

            Map(x => x.Date);
        }
    }
}
