using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class CitizenValidation : ValidationDef<Citizen>
    {
        public CitizenValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.CivilStatus);

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

            Define(x => x.CurrentAddress)
                .IsValid();

            Define(x => x.ProvincialAddress)
                .IsValid();

            Define(x => x.FingerPrint);

            Define(x => x.Pictures);

            Define(x => x.Signatures);
        }
    }
}
