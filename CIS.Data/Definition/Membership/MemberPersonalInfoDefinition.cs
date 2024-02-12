using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class MemberPersonalInfoDefinition
{
    public class Mapping : ClassMap<MemberPersonalInfo>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Foreign("MembershipInfo");

            HasOne(x => x.Member)
                .Constrained();

            Component(x => x.Person);

            Component(x => x.HomeAddress);

            Map(x => x.TelephoneNumber);

            Map(x => x.MobileNumber);

            Map(x => x.EmailAddress);

            Map(x => x.Height);

            Map(x => x.Weight);

            Map(x => x.BloodType);

            References(x => x.Religion);

            References(x => x.Citizenship);

            Map(x => x.EmergencyContactPerson);

            Map(x => x.EmergencyContactNumber);
        }
    }

    public class Validation : ValidationDef<MemberPersonalInfo>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Member);

            Define(x => x.Person)
                .IsValid();

            Define(x => x.HomeAddress)
                .IsValid();

            Define(x => x.TelephoneNumber)
                .MaxLength(15);

            Define(x => x.MobileNumber)
                .MaxLength(15);

            Define(x => x.EmailAddress)
                .MaxLength(150);

            Define(x => x.Height);

            Define(x => x.Weight);

            Define(x => x.BloodType);

            Define(x => x.Religion)
                .NotNullable();

            Define(x => x.Citizenship)
                .NotNullable();

            Define(x => x.EmergencyContactPerson)
                .MaxLength(150);

            Define(x => x.EmergencyContactNumber)
                .MaxLength(15);
        }
    }   
}
