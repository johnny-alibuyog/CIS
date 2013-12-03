using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Polices
{
    public class ApplicantValidation : ValidationDef<Applicant>
    {
        public ApplicantValidation()
        {
            Define(x => x.Id);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Mother)
                .IsValid();

            Define(x => x.Father)
                .IsValid();

            Define(x => x.Clearances)
                .HasValidElements();

            Define(x => x.Relatives)
                .HasValidElements();

            Define(x => x.Address)
                .NotNullable()
                .And.IsValid();

            Define(x => x.ProvincialAddress)
                .IsValid();

            Define(x => x.Picture);

            Define(x => x.Signature);

            Define(x => x.FingerPrint);

            Define(x => x.Height)
                .NotNullableAndNotEmpty()
                .And.MaxLength(20);

            Define(x => x.Weight)
                .NotNullableAndNotEmpty()
                .And.MaxLength(20);

            Define(x => x.Build)
                .MaxLength(150);

            Define(x => x.Marks)
                .MaxLength(150);

            Define(x => x.AlsoKnownAs)
                .MaxLength(150);

            Define(x => x.BirthPlace)
                .MaxLength(150);

            Define(x => x.Occupation)
                .MaxLength(150);

            Define(x => x.Religion)
                .MaxLength(150);

            Define(x => x.Citizenship)
                .MaxLength(100);

            Define(x => x.EmailAddress)
                .MaxLength(100);

            Define(x => x.TelephoneNumber)
                .MaxLength(100);

            Define(x => x.CellphoneNumber)
                .MaxLength(100);

            Define(x => x.PassportNumber)
                .MaxLength(50);

            Define(x => x.TaxIdentificationNumber)
                .MaxLength(50);

            Define(x => x.SocialSecuritySystemNumber)
                .MaxLength(50);

            Define(x => x.CivilStatus);
        }
    }
}
