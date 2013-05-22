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
                .MaxLength(150);

            Define(x => x.BirthPlace)
                .MaxLength(150);

            Define(x => x.Occupation)
                .MaxLength(150);

            Define(x => x.Religion)
                .MaxLength(150);

            Define(x => x.Citizenship)
                .MaxLength(100);

            Define(x => x.CivilStatus);

            Define(x => x.Purpose)
                .NotNullable();
        }
    }
}
