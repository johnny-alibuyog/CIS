using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Barangays.Maintenances
{
    public class MaintenanceMessage
    {
        public virtual string Identifier { get; set; }

        public MaintenanceMessage(string identifier)
        {
            Identifier = identifier;
        }
    }
}
