using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Polices
{
    public class ClearanceMapping : ClassMap<Clearance>
    {
        public ClearanceMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            Component(x => x.Person);

            Component(x => x.BodyStatistics);

            Component(x => x.Address);

            References(x => x.Picture)
                .Cascade.All();

            References(x => x.FingerPrint)
                .Cascade.All();

            References(x => x.Barcode)
                .Cascade.All();

            References(x => x.VerifiedBy);

            References(x => x.IssuedBy);

            Component(x => x.IssuedAt);

            Map(x => x.Office);

            Map(x => x.Station);

            Map(x => x.Location);

            Map(x => x.Purpose);

            Map(x => x.OfficialReceiptNumber);

            Map(x => x.CommunityTaxCertificateNumber);
        }
    }
}
