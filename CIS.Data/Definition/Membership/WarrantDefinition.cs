using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class WarrantDefinition
{
    public class Mapping : ClassMap<Warrant>
    {
        public Mapping()
        {
            //OptimisticLock.Version();

            Id(x => x.Id);

            //Version(x => x.Version);

            Map(x => x.DataStoreParentKey)
                .Index("DataStoreParentKeyIndex");

            Component(x => x.Audit);

            Map(x => x.WarrantCode)
                .Index("WarrantCodeIndex");

            Map(x => x.CaseNumber);

            Map(x => x.Crime);

            Map(x => x.Description);

            Map(x => x.Remarks);

            Map(x => x.BailAmount)
                .Precision(25)
                .Scale(4);

            Map(x => x.IssuedOn);

            Map(x => x.IssuedBy);

            Component(x => x.IssuedAt)
                .ColumnPrefix("IssuedAt");

            HasMany(x => x.Suspects)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();
        }
    }

    public class Validation : ValidationDef<Warrant>
    {
        public Validation()
        {
            Define(x => x.Id);

            //Define(x => x.Version);

            Define(x => x.DataStoreParentKey);

            Define(x => x.Audit);

            Define(x => x.WarrantCode)
                .MaxLength(50);

            Define(x => x.CaseNumber)
                .MaxLength(50);

            Define(x => x.Crime)
                .MaxLength(300);

            Define(x => x.Description)
                .MaxLength(4001);

            Define(x => x.Remarks)
                .MaxLength(4001);

            Define(x => x.BailAmount);

            Define(x => x.IssuedOn);
            //.IsInThePast();

            Define(x => x.IssuedBy)
                .MaxLength(300);

            Define(x => x.IssuedAt)
                .IsValid();

            Define(x => x.Suspects)
                .NotNullableAndNotEmpty()
                .And.HasValidElements();
        }
    }
}
