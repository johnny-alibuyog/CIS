using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Validations.Barangays
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

            Define(x => x.Address)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Officials)
                .NotNullableAndNotEmpty()
                .And.HasValidElements();

            Define(x => x.Purpose)
                .NotNullableAndNotEmpty()
                .And.MaxLength(100);

            Define(x => x.ClearanceFee);

            Define(x => x.CommunityTaxCertificateNumber)
                .MaxLength(100);

            Define(x => x.OfficialReceiptNumber)
                .MaxLength(100);

            Define(x => x.Date);
        }
    }
}
