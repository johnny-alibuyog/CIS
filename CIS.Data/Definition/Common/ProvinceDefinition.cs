using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class ProvinceDefinition
{
    public class Mapping : ClassMap<Province>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name)
                .Index("ProvinceNameIndex");

            References(x => x.Region);

            HasMany(x => x.Cities)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();
        }

        public class Validation : ValidationDef<Province>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(100);

                Define(x => x.Region)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.Cities)
                    .HasValidElements();
            }
        }
    }
}
