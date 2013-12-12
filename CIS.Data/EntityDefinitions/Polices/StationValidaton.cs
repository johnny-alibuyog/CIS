using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Polices;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Polices
{
    public class StationValidaton : ValidationDef<Station>
    {
        public StationValidaton()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Logo);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Office)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Location)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.ClearanceFee);

            Define(x => x.ClearanceValidityInDays);

            Define(x => x.Address)
                .IsValid();

            Define(x => x.Officers)
                .HasValidElements();
        }
    }
}
