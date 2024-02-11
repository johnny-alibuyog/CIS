using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class OfficerDefinition
{
    public class Mapping : ClassMap<Officer>
    {
        public Mapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            Component(x => x.Person);

            References(x => x.Station);

            References(x => x.Rank);

            Map(x => x.Position);

            References(x => x.Signature)
                .Cascade.All()
                .Fetch.Join();
        }
    }

    public class Validation : ValidationDef<Officer>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Station)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Rank)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Position)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Signature)
                .IsValid();
        }
    }
}
