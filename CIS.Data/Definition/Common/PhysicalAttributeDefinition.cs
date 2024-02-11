using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class PhysicalAttributeDefinition
{
    public class Mapping : ComponentMap<PhysicalAttribute>
    {
        public Mapping()
        {
            Map(x => x.Hair);

            Map(x => x.Eyes);

            Map(x => x.Complexion);

            Map(x => x.Build);

            Map(x => x.ScarsAndMarks);

            Map(x => x.Race);

            Map(x => x.Nationality);
        }
    }

    public class Validation : ValidationDef<PhysicalAttribute>
    {
        public Validation()
        {
            Define(x => x.Hair)
                .MaxLength(250);

            Define(x => x.Eyes)
                .MaxLength(250);

            Define(x => x.Complexion)
                .MaxLength(250);

            Define(x => x.Build)
                .MaxLength(250);

            Define(x => x.ScarsAndMarks)
                .MaxLength(250);

            Define(x => x.Race)
                .MaxLength(250);

            Define(x => x.Nationality)
                .MaxLength(250);
        }
    }
}
