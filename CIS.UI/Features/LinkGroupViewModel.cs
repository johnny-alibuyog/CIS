using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CIS.UI.Features
{
    public class LinkGroupViewModel
    {
        public virtual string DisplayName { get; set; }
        public virtual IReactiveList<LinkViewModel> Links { get; set; }
    }
}
