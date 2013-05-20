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

            Define(x => x.Address)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Picture);

            Define(x => x.FingerPrint);

            Define(x => x.Height)
                .NotNullableAndNotEmpty()
                .And.MaxLength(20);

            Define(x => x.Weight)
                .NotNullableAndNotEmpty()
                .And.MaxLength(20);

            Define(x => x.AlsoKnownAs)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.BirthPlace)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Occupation)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Religion)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Purpose)
                .NotNullable();
        }
    }
}
