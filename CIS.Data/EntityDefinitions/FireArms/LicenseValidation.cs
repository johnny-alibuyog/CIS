using CIS.Core.Entities.Firearms;
using NHibernate.Validator.Cfg.Loquacious;
using System;

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

            ValidateInstance.By((instance, context) =>
            {
                var result = true;
                var sqlMinimumDate = new DateTime(1900, 1, 1);

                if (instance.IssueDate < sqlMinimumDate)
                {
                    var message = string.Format("License {0} has invalid issue date.", instance.LicenseNumber);
                    context.AddInvalid<License, DateTime?>(message, x => x.IssueDate);
                    result = false;
                }

                if (instance.ExpiryDate < sqlMinimumDate)
                {
                    var message = string.Format("License {0} has invalid expiry date.", instance.LicenseNumber);
                    context.AddInvalid<License, DateTime?>(message, x => x.ExpiryDate);
                    result = false;
                }

                if (instance.Person.BirthDate < sqlMinimumDate)
                {
                    var message = string.Format("License {0} has invalid birth date.", instance.LicenseNumber);
                    context.AddInvalid<License, DateTime?>(message, x => x.Person.BirthDate);
                    result = false;
                }

                return result;
            });
        }
    }
}
