using CIS.Core.Domain.Membership;
using CIS.Data.Common.Extention;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class SettingDefinition
{
    public class Mapping : ClassMap<Setting>
    {
        public Mapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            References(x => x.Terminal);

            Map(x => x.WithCameraDevice);

            Map(x => x.WithFingerScannerDevice);

            Map(x => x.WithDigitalSignatureDevice);

            HasManyToMany(x => x.FingersToScan)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("SettingsFingersToScan")
                .Cascade.SaveUpdate()
                .AsSet();

            References(x => x.CurrentVerifier);

            References(x => x.CurrentCertifier);
        }
    }

    public class Validation : ValidationDef<Setting>
    {
        public Validation()
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

            Define(x => x.CurrentVerifier);

            Define(x => x.CurrentCertifier);
        }
    }
}
