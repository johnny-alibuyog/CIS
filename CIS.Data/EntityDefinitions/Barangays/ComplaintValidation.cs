using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Barangays
{
    public class ComplaintValidation : ValidationDef<Complaint>
    {
        public ComplaintValidation()
        {
            Define(x => x.Id);

            Define(x => x.Description)
                .MaxLength(100);

            Define(x => x.DetailedSummary)
                .MaxLength(2500);
        }
    }
}
