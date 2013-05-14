using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Polices;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Polices
{
    public class OfficerValidation : ValidationDef<Officer>
    {
        public OfficerValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Station)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Rank)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Position)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Title)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);
        }
    }
}
