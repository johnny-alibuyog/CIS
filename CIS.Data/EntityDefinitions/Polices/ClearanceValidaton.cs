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

            Define(x => x.VerifiedBy)
                .NotNullable()
                .And.IsValid();

            Define(x => x.IssuedBy)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Office)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Station)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Location)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.OfficialReceiptNumber)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);

            Define(x => x.CommunityTaxCertificateNumber)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);


            Define(x => x.PartialMatchFindings)
                .MaxLength(1000);

            Define(x => x.PerfectMatchFindings)
                .MaxLength(1000);
        }
    }
}
