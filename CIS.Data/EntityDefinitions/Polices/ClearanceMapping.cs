using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Polices
{
    public class ClearanceMapping : ClassMap<Clearance>
    {
        public ClearanceMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            References(x => x.Applicant)
                .Cascade.All();

            References(x => x.Barcode)
                .Cascade.All();

            References(x => x.VerifiedBy);

            References(x => x.IssuedBy);

            Component(x => x.IssuedAt)
                .ColumnPrefix("IssuedAt");

            Map(x => x.Office);

            Map(x => x.Station);

            Map(x => x.Location);

            Map(x => x.OfficialReceiptNumber);

            Map(x => x.CommunityTaxCertificateNumber);

            Map(x => x.PartialMatchFindings);

            Map(x => x.PerfectMatchFindings);
        }
    }
}
