﻿using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Barangays;

public class OfficeValidation : ValidationDef<Office>
{
    public OfficeValidation()
    {
        Define(x => x.Id);

        Define(x => x.Version);

        Define(x => x.Audit);

        Define(x => x.Logo);

        Define(x => x.Name)
            .NotNullableAndNotEmpty()
            .And.MaxLength(250);

        Define(x => x.Location)
            .NotNullableAndNotEmpty()
            .And.MaxLength(250);

        Define(x => x.Address)
            .NotNullable()
            .And.IsValid();

        Define(x => x.ClearanceFee);

        Define(x => x.CertificationFee);

        Define(x => x.DocumentStampTax);

        Define(x => x.Incumbents)
            .HasValidElements();
    }
}
