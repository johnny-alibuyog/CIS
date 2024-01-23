using CIS.Core.Entities.Polices;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Polices;

public class WarrantValidation : ValidationDef<Warrant>
{
    public WarrantValidation()
    {
        Define(x => x.Id);

        //Define(x => x.Version);

        Define(x => x.DataStoreParentKey);

        Define(x => x.Audit);

        Define(x => x.WarrantCode)
            .MaxLength(50);

        Define(x => x.CaseNumber)
            .MaxLength(50);

        Define(x => x.Crime)
            .MaxLength(300);

        Define(x => x.Description)
            .MaxLength(4001);

        Define(x => x.Remarks)
            .MaxLength(4001);

        Define(x => x.BailAmount);

        Define(x => x.IssuedOn);
            //.IsInThePast();

        Define(x => x.IssuedBy)
            .MaxLength(300);

        Define(x => x.IssuedAt)
            .IsValid();

        Define(x => x.Suspects)
            .NotNullableAndNotEmpty()
            .And.HasValidElements();
    }
}
