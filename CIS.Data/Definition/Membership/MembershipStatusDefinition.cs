using CIS.Core.Domain.Membership;
using CIS.Data.Common.Extention;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class MembershipStatusDefinition
{
    public class Mapping : ClassMap<MembershipStatus>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.MembershipInfo);

            DiscriminateSubClassesOnColumn("Type");
        }
    }

    public class Validation : ValidationDef<MembershipStatus>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.MembershipInfo);
        }
    }
}

public class MemberAppliedDefinition
{
    public class Mapping : SubclassMap<MemberApplied>
    {
        public Mapping()
        {
            DiscriminatorValue(nameof(MemberApplied));

            Map(x => x.AppliedOn);

            HasManyToMany(x => x.Signatories)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("MemberApprovedSignatories")
                .Cascade.AllDeleteOrphan()
                .AsSet();
        }
    }

    public class Validation : ValidationDef<MemberApplied>
    {
        public Validation()
        {
            Define(x => x.AppliedOn);

            Define(x => x.Signatories)
                .HasValidElements();
        }
    }
}

public class MemberApprovedDefinition
{
    public class Mapping : SubclassMap<MemberApproved>
    {
        public Mapping()
        {
            DiscriminatorValue(nameof(MemberApproved));

            Map(x => x.ApprovedOn);

            HasManyToMany(x => x.Signatories)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("MemberApprovedSignatories")
                .Cascade.AllDeleteOrphan()
                .AsSet();
        }
    }

    public class Validation : ValidationDef<MemberApproved>
    {
        public Validation()
        {
            Define(x => x.ApprovedOn);

            Define(x => x.Signatories)
                .HasValidElements();
        }
    }
}

public class MemberRejectedDefinition
{
    public class Mapping : SubclassMap<MemberRejected>
    {
        public Mapping()
        {
            DiscriminatorValue(nameof(MemberRejected));

            Map(x => x.RejectedOn);

            Map(x => x.Reason);
        }
    }

    public class Validation : ValidationDef<MemberRejected>
    {
        public Validation()
        {
            Define(x => x.RejectedOn);

            Define(x => x.Reason)
                .MaxLength(600);
        }
    }
}

public class MemberRegisteredDefinition
{
    public class Mapping : SubclassMap<MemberRegistered>
    {
        public Mapping()
        {
            DiscriminatorValue(nameof(MemberRegistered));

            Map(x => x.Year);

            Map(x => x.RegisteredOn);

            References(x => x.IncumbentSecretary);

            References(x => x.IncumbentPersident);
        }
    }

    public class Validation : ValidationDef<MemberRegistered>
    {
        public Validation()
        {
            Define(x => x.Year);

            Define(x => x.RegisteredOn);

            Define(x => x.IncumbentSecretary)
                .NotNullable();

            Define(x => x.IncumbentPersident)
                .NotNullable();
        }
    }
}
