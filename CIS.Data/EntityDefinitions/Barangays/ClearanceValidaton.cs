using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Barangays;

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

        Define(x => x.ApplicantPicture);

        Define(x => x.ApplicantSignature);

        Define(x => x.ApplicantAddress)
            .MaxLength(700);

        Define(x => x.Office);

        Define(x => x.Officials)
            .HasValidElements();

        Define(x => x.ApplicationDate);

        Define(x => x.IssueDate);

        Define(x => x.Fee);

        Define(x => x.ControlNumber)
            .MaxLength(50);

        Define(x => x.OfficialReceiptNumber)
            .MaxLength(50);

        Define(x => x.TaxCertificateNumber)
            .MaxLength(50);

        Define(x => x.FinalFindings)
            .MaxLength(2000);

        Define(x => x.Finding);

        Define(x => x.Purpose)
            .NotNullable();
    }
}