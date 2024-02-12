using CIS.Core.Domain.Membership;
using CIS.Data.Common.Extention;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class MemberDependentInfoDefinition
{
    public class Mapping : ClassMap<MemberDependentInfo>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Foreign("MembershipInfo");

            HasOne(x => x.Member)
                .Constrained();

            HasManyToMany(x => x.Dependents)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("MemberDependentInfoDependents")
                .Cascade.AllDeleteOrphan()
                .AsSet();
        }
    }

    public class Validation : ValidationDef<MemberDependentInfo>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Member);

            Define(x => x.Dependents)
                .NotNullable();
        }
    }
}
