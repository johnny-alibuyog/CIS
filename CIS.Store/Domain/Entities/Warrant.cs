using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS.Store.Domain.Entities
{
    public class Warrant
    {
        public virtual long Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Crime { get; set; }
        public virtual string SuspectFirstName { get; set; }
        public virtual string SuspectMiddleName { get; set; }
        public virtual string SuspectLastName { get; set; }
        public virtual string SuspectSuffix { get; set; }
    }
}