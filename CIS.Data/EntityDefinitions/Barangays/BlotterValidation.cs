using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Barangays;

public class BlotterValidation : ValidationDef<Blotter>
{
    public BlotterValidation()
    {
        Define(x => x.Id);

        Define(x => x.Version);

        Define(x => x.Audit);

        Define(x => x.Complaint)
            .NotNullableAndNotEmpty()
            .And.MaxLength(200);

        Define(x => x.Details)
            .MaxLength(2000);

        Define(x => x.Remarks)
            .MaxLength(2000);

        Define(x => x.Status);

        Define(x => x.FiledOn);

        Define(x => x.OccuredOn);

        Define(x => x.Address);

        Define(x => x.Incumbent)
            .NotNullable()
            .And.IsValid();

        Define(x => x.Officials)
            .NotNullableAndNotEmpty()
            .And.HasValidElements();

        Define(x => x.Complainants)
            .NotNullableAndNotEmpty()
            .And.HasValidElements();

        Define(x => x.Respondents)
            .NotNullableAndNotEmpty()
            .And.HasValidElements();

        Define(x => x.Witnesses)
            .HasValidElements();
    }
}
