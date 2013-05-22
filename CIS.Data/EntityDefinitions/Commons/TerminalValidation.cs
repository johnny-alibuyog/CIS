using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Commons
{
    public class TerminalValidation : ValidationDef<Terminal>
    {
        public TerminalValidation()
        {
            Define(x => x.Id);

            Define(x => x.MachineName)
                .MaxLength(100);

            Define(x => x.IpAddress)
                .MaxLength(100);

            Define(x => x.MacAddress)
                .MaxLength(100);

            Define(x => x.WithDefaultLogin);

            Define(x => x.WithFingerPrintDevice);

            Define(x => x.FingersToScan)
                .HasValidElements();
        }
    }
}
