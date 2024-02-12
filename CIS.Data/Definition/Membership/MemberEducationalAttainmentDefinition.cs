using CIS.Core.Domain.Membership;
using CIS.Data.Common.Extention;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class MemberEducationalAttainmentDefinition
{
    public class Mapping : ClassMap<MemberEducationalAttainment>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Foreign("MembershipInfo");

            HasOne(x => x.Member)
                .Constrained();

            HasManyToMany(x => x.Educations)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("MemberEducationalAttainmentEducations")
                .Cascade.AllDeleteOrphan()
                .AsSet();
        }
    }

    public class Validation : ValidationDef<MemberEducationalAttainment>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Member);
        }
    }
}
