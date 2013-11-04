using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Polices
{
    public class AmendmentValidation : ValidationDef<Amendment>
    {
        public AmendmentValidation()
        {
            Define(x => x.Id);

            Define(x => x.DocumentNumber)
                .MaxLength(50);

            Define(x => x.Reason)
                .MaxLength(250);

            Define(x => x.Remarks)
                .MaxLength(250);
        }
    }
}
