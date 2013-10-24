using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Constraints;

namespace CIS.UI.Features
{
    public class Window1ChildViewModel : ViewModelBase
    {
        [NotNullNotEmpty]
        public virtual string ChildText1 { get; set; }

        [NotNullNotEmpty]
        public virtual string ChildText2 { get; set; }

        [NotNullNotEmpty]
        public virtual string ChildText3 { get; set; }

    }
}
