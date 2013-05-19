using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Firearms;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.FireArms
{
    public class LicenseValidation : ValidationDef<License>
    {
        public LicenseValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Address)
                .IsValid();

            Define(x => x.Gun)
                .NotNullable()
                .And.IsValid();

            Define(x => x.LicenseNumber)
                .NotNullableAndNotEmpty()
                .And.MaxLength(100);

            Define(x => x.ControlNumber)
                .NotNullableAndNotEmpty()
                .And.MaxLength(100);

            Define(x => x.IssueDate);

            Define(x => x.ExpiryDate);
        }
    }
}
