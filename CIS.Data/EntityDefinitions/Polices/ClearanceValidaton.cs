using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Polices;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Polices
{
    public class ClearanceValidaton : ValidationDef<Clearance>
    {
        public ClearanceValidaton()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Applicant)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Barcode)
                .IsValid();

            Define(x => x.Verifier)
                .NotNullable()
                .And.IsValid();

            Define(x => x.VerifierRank)
                .MaxLength(100);

            Define(x => x.VerifierPosition)
                .MaxLength(100);

            Define(x => x.Certifier)
                .NotNullable()
                .And.IsValid();

            Define(x => x.CertifierRank)
                .MaxLength(100);

            Define(x => x.CertifierPosition)
                .MaxLength(100);

            Define(x => x.Station)
                .NotNullable()
                .And.IsValid();

            Define(x => x.IssueDate);

            Define(x => x.Validity)
                .NotNullableAndNotEmpty()
                .And.MaxLength(250);

            Define(x => x.OfficialReceiptNumber)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);

            Define(x => x.TaxCertificateNumber)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);


            Define(x => x.PartialMatchFindings)
                .MaxLength(2000);

            Define(x => x.PerfectMatchFindings)
                .MaxLength(2000);

            Define(x => x.FinalFindings)
                .MaxLength(2000);

            Define(x => x.SuspectPartialMatches)
                .HasValidElements();

            Define(x => x.SuspectPerfectMatches)
                .HasValidElements();

            Define(x => x.ExpiredLicenseMatches)
                .HasValidElements();

        }
    }
}
