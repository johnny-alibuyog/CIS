using CIS.Core.Domain.Common;
using CIS.Data.Common.Extention;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class ConfigurationDefinition
{
    public class Mapping : ClassMap<Configuration>
    {
        public Mapping()
        {
            Id(x => x.Id);

            HasMany(x => x.Properties)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Schema(GetType().ParseSchema())
                .Table("ConfigurationValues")
                .Fetch.Join()
                .AsMap<string>(
                    index => index.Column("Identifier").Type<string>(),
                    element => element.Column("Value").Type<string>().Length(750)
                );

            DiscriminateSubClassesOnColumn("Discriminator")
                .Index("DiscriminatorIndex")
                .Not.Nullable()
                .Length(50);
        }
    }

    public class Validation : ValidationDef<Configuration>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Properties);
        }
    }
}
