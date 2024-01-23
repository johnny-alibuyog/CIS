using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons;

public class BarcodeValidaton : ValidationDef<Barcode>
{
    public BarcodeValidaton()
    {
        Define(x => x.Id);

        Define(x => x.Image)
            .NotNullable()
            .And.IsValid();

        Define(x => x.Text)
            .NotNullableAndNotEmpty()
            .And.MaxLength(150);

    }
}
