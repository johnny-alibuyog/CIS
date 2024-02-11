using CIS.Core.Domain.Membership;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class RegistrationDefinition
{
    public class Mapping : ClassMap<Registration>
    {
        public Mapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            References(x => x.Application)
                .Cascade.All();

            References(x => x.ApplicantPicture);

            References(x => x.ApplicantSignature);

            Map(x => x.ApplicantCivilStatus);

            Map(x => x.ApplicantAddress);

            Map(x => x.ApplicantCitizenship);

            References(x => x.Barcode)
                .Cascade.All();

            References(x => x.Verifier);

            Map(x => x.VerifierRank);

            Map(x => x.VerifierPosition);

            References(x => x.Certifier);

            Map(x => x.CertifierRank);

            Map(x => x.CertifierPosition);

            References(x => x.Station);

            Map(x => x.ApplicationDate);

            Map(x => x.IssueDate);

            Map(x => x.Validity);

            Map(x => x.ControlNumber);

            Map(x => x.OfficialReceiptNumber);

            Map(x => x.TaxCertificateNumber);

            Map(x => x.Fee);

            Map(x => x.YearsResident);

            Map(x => x.FinalFindings);

            References(x => x.Finding)
                .Cascade.All();

            References(x => x.Purpose);
        }
    }

    public class Validaton : ValidationDef<Registration>
    {
        public Validaton()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Application)
                .NotNullable()
                .And.IsValid();

            Define(x => x.ApplicantPicture);

            Define(x => x.ApplicantSignature);

            Define(x => x.ApplicantCivilStatus);

            Define(x => x.ApplicantAddress)
                .MaxLength(700);

            Define(x => x.ApplicantCitizenship)
                .MaxLength(100);

            Define(x => x.Barcode)
                .IsValid();

            Define(x => x.Verifier)
                .NotNullable()
                .And.IsValid();

            Define(x => x.VerifierRank)
                .MaxLength(100);

            Define(x => x.VerifierPosition)
                .MaxLength(100);

            Define(x => x.Certifier)
                .NotNullable()
                .And.IsValid();

            Define(x => x.CertifierRank)
                .MaxLength(100);

            Define(x => x.CertifierPosition)
                .MaxLength(100);

            Define(x => x.Station)
                .NotNullable()
                .And.IsValid();

            Define(x => x.ApplicationDate);

            Define(x => x.IssueDate);

            Define(x => x.Validity)
                .NotNullableAndNotEmpty()
                .And.MaxLength(250);

            Define(x => x.ControlNumber)
                .MaxLength(50);

            Define(x => x.OfficialReceiptNumber)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);

            Define(x => x.TaxCertificateNumber)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);

            Define(x => x.Fee);

            Define(x => x.YearsResident);

            Define(x => x.FinalFindings)
                .MaxLength(2000);

            Define(x => x.Finding);

            Define(x => x.Purpose)
                .NotNullable();
        }
    }
}
