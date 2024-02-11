using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class FindingDefinition
{
    public class Mapping : ClassMap<Finding>
    {
        public Mapping()
        {
            Id(x => x.Id);

            Map(x => x.FinalFindings);

            References(x => x.Amendment)
                .Cascade.All();

            HasMany(x => x.Hits)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();
        }
    }

    public class Validation : ValidationDef<Finding>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.FinalFindings)
                .MaxLength(500);

            Define(x => x.Amendment);

            Define(x => x.Hits)
                .HasValidElements();
        }
    }
}
