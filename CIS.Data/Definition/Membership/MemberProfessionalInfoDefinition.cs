using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class MemberProfessionalInfoDefinition
{
    public class Mapping : ClassMap<MemberProfessionalInfo>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Foreign("MembershipInfo");

            HasOne(x => x.Member)
                .Constrained();

            Map(x => x.Company);

            Map(x => x.LineOfBusiness);

            Map(x => x.TitleAndPosition);

            Component(x => x.CompanyAddress);

            Map(x => x.TelephoneNumber);

            Map(x => x.FaxNumber);

            Map(x => x.GSIS);

            Map(x => x.Pagibig);

            Map(x => x.PhilHealth);

            Map(x => x.TIN);
        }
    }

    public class Validation : ValidationDef<MemberProfessionalInfo>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Member);

            Define(x => x.Company)
                .MaxLength(100);

            Define(x => x.LineOfBusiness)
                .MaxLength(100);

            Define(x => x.TitleAndPosition)
                .MaxLength(100);

            Define(x => x.CompanyAddress);

            Define(x => x.TelephoneNumber)
                .MaxLength(15);

            Define(x => x.FaxNumber)
                .MaxLength(15);

            Define(x => x.GSIS)
                .MaxLength(25);

            Define(x => x.Pagibig)
                .MaxLength(25);

            Define(x => x.PhilHealth)
               .MaxLength(25);

            Define(x => x.TIN)
                .MaxLength(25);
        }
    }
}
