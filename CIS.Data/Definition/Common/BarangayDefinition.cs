using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class BarangayDefinition
{
    public class Mapping: ClassMap<Barangay>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name)
                .Index("BarangayNameIndex");

            References(x => x.City);

            Map(x => x.AreaClass);

            Map(x => x.Population);
        }
    }

    public class Validation : ValidationDef<Barangay>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(100);

            Define(x => x.City)
                .NotNullable()
                .And.IsValid();

            Define(x => x.AreaClass);

            Define(x => x.Population);
        }
    }
}
