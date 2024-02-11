using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class CityDefinition
{
    public class Mapping : ClassMap<City>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name)
                .Index("CityNameIndex");

            References(x => x.Province);

            HasMany(x => x.Barangays)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();
        }
    }

    public class Validation : ValidationDef<City>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(100);

            Define(x => x.Province)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Barangays)
                .HasValidElements();
        }
    }
}
