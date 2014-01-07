using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Polices;
using CIS.Data.Commons.Extentions;
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

            References(x => x.ApplicantPicture);

            References(x => x.ApplicantSignature);

            Map(x => x.ApplicantCivilStatus);

            Map(x => x.ApplicantAddress);

            Map(x => x.ApplicantCitizenship);

            References(x => x.Barcode)
                .Cascade.All();

            References(x => x.Verifier);

            Map(x => x.VerifierRank);

            Map(x => x.VerifierPosition);

            References(x => x.Certifier);

            Map(x => x.CertifierRank);

            Map(x => x.CertifierPosition);

            References(x => x.Station);

            Map(x => x.ApplicationDate);

            Map(x => x.IssueDate);

            Map(x => x.Validity);

            Map(x => x.ControlNumber);

            Map(x => x.OfficialReceiptNumber);

            Map(x => x.TaxCertificateNumber);

            Map(x => x.Fee);

            Map(x => x.YearsResident);

            Map(x => x.FinalFindings);

            References(x => x.Finding)
                .Cascade.All();

            References(x => x.Purpose);
        }
    }
}
