using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class JobDescriptionValidation : ValidationDef<JobDescription>
    {
        public JobDescriptionValidation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Position)
                .NotNullable()
                .And.IsValid();
        }
    }
}
