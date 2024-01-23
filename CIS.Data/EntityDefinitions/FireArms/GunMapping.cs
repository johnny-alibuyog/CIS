using CIS.Core.Entities.Firearms;
using FluentNHibernate.Mapping;
using System;

namespace CIS.Data.EntityDefinitions.FireArms;

public class GunMapping : ComponentMap<Gun>
{
    public GunMapping()
    {
        Map(x => x.Model);

        Map(x => x.Caliber);

        Map(x => x.SerialNumber);

        References(x => x.Kind);

        References(x => x.Make);
    }

    internal static Action<ComponentPart<Gun>> Map(string columnPrefix = "")
    {
        return mapping =>
        {
            mapping.Map(x => x.Model, columnPrefix + "Model");

            mapping.Map(x => x.Caliber, columnPrefix + "Caliber");

            mapping.Map(x => x.SerialNumber, columnPrefix + "SerialNumber");

            mapping.References(x => x.Kind, columnPrefix + "KindId");

            mapping.References(x => x.Make, columnPrefix + "MakeId");
        };
    }
}
