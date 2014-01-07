using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Polices;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Polices
{
    public class SuspectValidation : ValidationDef<Suspect>
    {
        public SuspectValidation()
        {
            Define(x => x.Id);

            Define(x => x.DataStoreId);

            Define(x => x.DataStoreChildKey);

            //Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Warrant)
                .IsValid();

            Define(x => x.ArrestStatus);

            Define(x => x.ArrestDate);

            Define(x => x.Disposition)
                .MaxLength(700);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.PhysicalAttributes)
                .IsValid();

            Define(x => x.Address)
                .IsValid();

            Define(x => x.Aliases);

            Define(x => x.Occupations);

        }
    }
}
