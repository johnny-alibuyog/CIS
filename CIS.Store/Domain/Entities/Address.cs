using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS.Store.Domain.Entities
{
    public class Address
    {
        public virtual string Address1 { get; set; }

        public virtual string Address2 { get; set; }

        public virtual string Barangay { get; set; }

        public virtual string City { get; set; }

        public virtual string Province { get; set; }
    }
}