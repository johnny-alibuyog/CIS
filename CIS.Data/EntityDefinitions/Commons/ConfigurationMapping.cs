using CIS.Core.Entities.Commons;
using CIS.Data.Commons.Extentions;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Commons;

public class ConfigurationMapping : ClassMap<Configuration>
{
    public ConfigurationMapping()
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
