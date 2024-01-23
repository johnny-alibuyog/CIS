using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Barangays;

public class SettingValidation : ValidationDef<Setting>
{
    public SettingValidation()
    {
        Define(x => x.Id);

        Define(x => x.Version);

        Define(x => x.Audit);

        Define(x => x.Terminal)
            .NotNullable()
            .And.IsValid();

        Define(x => x.WithCameraDevice);

        Define(x => x.WithFingerScannerDevice);

        Define(x => x.WithDigitalSignatureDevice);

        Define(x => x.FingersToScan)
            .HasValidElements();
    }
}
