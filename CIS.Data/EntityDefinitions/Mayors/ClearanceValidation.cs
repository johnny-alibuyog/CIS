using CIS.Core.Entities.Mayors;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Mayors;

public class ClearanceValidation : ValidationDef<Clearance>
{
    public ClearanceValidation()
    {
        Define(x => x.Id);

        Define(x => x.Version);

        Define(x => x.Audit);

        Define(x => x.Applicant)
            .IsValid();

        Define(x => x.ApplicantAddress)
            .IsValid();

        Define(x => x.ApplicantPicture);

        Define(x => x.Company);

        Define(x => x.CompanyAddress)
            .IsValid();

        Define(x => x.Mayor)
            .IsValid();

        Define(x => x.Secretary)
            .IsValid();

        Define(x => x.IssueDate);

        Define(x => x.PermitNumber)
            .MaxLength(100);

        Define(x => x.PermitFee);

        Define(x => x.Penalty);

        Define(x => x.OfficialReceiptNumber)
            .MaxLength(100);

        Define(x => x.Notice)
            .MaxLength(4001);
    }
}
