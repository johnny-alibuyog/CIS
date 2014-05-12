using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Barangays
{
    public class OfficialValidation : ValidationDef<Official>
    {
        public OfficialValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Position)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Committee)
                .IsValid();

            Define(x => x.Incumbent)
                .IsValid();

            Define(x => x.Picture)
                .IsValid();

            Define(x => x.Signature)
                .IsValid();

            Define(x => x.IsActive);

            ValidateInstance.By((instance, context) =>
            {
                var result = true;

                if (instance.Committee == null && instance.Position.Committees.Count() > 0)
                {
                    var message = string.Format("Committee for {0} is mandatory.", instance.Position.Name);
                    context.AddInvalid<Official, Committee>(message, x => x.Committee);
                    result = false;
                }

                return result;
            });
        }
    }
}
