using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class MemberDefinition
{
    public class Mapping : ClassMap<Member>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            HasOne(x => x.PersonalInfo)
                .Cascade.All();

            HasOne(x => x.ProfessionalInfo)
                .Cascade.All();

            HasOne(x => x.DependentInfo)
                .Cascade.All();

            HasOne(x => x.EducationalAttainment)
                .Cascade.All();

            HasOne(x => x.MembershipInfo)
                .Cascade.All();
        }
    }

    public class Validation : ValidationDef<Member>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.PersonalInfo)
                .NotNullable()
                .And.IsValid();

            Define(x => x.ProfessionalInfo)
                .NotNullable()
                .And.IsValid();

            Define(x => x.DependentInfo)
                .NotNullable()
                .And.IsValid();

            Define(x => x.EducationalAttainment)
                .NotNullable()
                .And.IsValid();

            Define(x => x.MembershipInfo)
                .NotNullable()
                .And.IsValid();
        }
    }   
}
