using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features
{
    public class LinkViewModel : ViewModelBase
    {
        public virtual string DisplayName { get; set; }
        public virtual Uri Source { get; set; }
    }
}
