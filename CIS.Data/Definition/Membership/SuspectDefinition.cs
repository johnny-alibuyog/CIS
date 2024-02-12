using CIS.Core.Domain.Membership;
using CIS.Data.Common.Extention;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class SuspectDefinition
{
    public class SuspectMapping : ClassMap<Suspect>
    {
        public SuspectMapping()
        {
            //OptimisticLock.Version();

            Id(x => x.Id);

            Map(x => x.DataStoreId)
                .Index("DataStoreIdIndex");

            Map(x => x.DataStoreChildKey)
                .Index("DataStoreChildKey");

            //Version(x => x.Version);

            Component(x => x.Audit);

            References(x => x.Warrant);

            Map(x => x.ArrestStatus);

            Map(x => x.ArrestDate);

            Map(x => x.Disposition);

            Component(x => x.Person);

            Component(x => x.Address);

            Component(x => x.PhysicalAttributes);

            HasMany(x => x.Aliases)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Cascade.AllDeleteOrphan()
                .Fetch.Subselect()
                .Table("SuspectAliases")
                .Element("Descrition")
                .AsSet();

            HasMany(x => x.Occupations)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Cascade.AllDeleteOrphan()
                .Fetch.Subselect()
                .Table("SuspectOccupations")
                .Element("Value")
                .AsSet();
        }
    }

    public class SuspectValidation : ValidationDef<Suspect>
    {
        public SuspectValidation()
        {
            Define(x => x.Id);

            Define(x => x.DataStoreId);

            Define(x => x.DataStoreChildKey);

            //Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Warrant)
                .IsValid();

            Define(x => x.ArrestStatus);

            Define(x => x.ArrestDate);

            Define(x => x.Disposition)
                .MaxLength(700);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.PhysicalAttributes)
                .IsValid();

            Define(x => x.Address)
                .IsValid();

            Define(x => x.Aliases);

            Define(x => x.Occupations);

        }
    }
}
