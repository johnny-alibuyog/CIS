using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features.Polices.Clearances
{
    public class ArchiveReportViewModel : ViewModelBase
    {
        public virtual IList<ArchiveItemViewModel> Items { get; set; }
    }
}
