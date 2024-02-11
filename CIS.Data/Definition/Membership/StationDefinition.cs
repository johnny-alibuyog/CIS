using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class StationDefinition
{
    public class Mapping : ClassMap<Station>
    {
        public Mapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            References(x => x.Logo)
                .Cascade.All()
                .Fetch.Join();

            Map(x => x.Name);

            Map(x => x.Office);

            Map(x => x.Location);

            Map(x => x.ClearanceFee);

            Map(x => x.ClearanceValidityInDays);

            Component(x => x.Address);

            HasMany(x => x.Officers)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();
        }
    }

    public class Validaton : ValidationDef<Station>
    {
        public Validaton()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Logo);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Office)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Location)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.ClearanceFee);

            Define(x => x.ClearanceValidityInDays);

            Define(x => x.Address)
                .IsValid();

            Define(x => x.Officers)
                .HasValidElements();
        }
    }
}
