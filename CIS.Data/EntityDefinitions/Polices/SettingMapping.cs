using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using CIS.Data.Commons.Extentions;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Polices
{
    public class SettingMapping : ClassMap<Setting>
    {
        public SettingMapping()
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
}
