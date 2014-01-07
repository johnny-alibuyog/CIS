using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Store.Domain.Entities;

namespace CIS.Store.Services.Warrants
{
    public class PushWarrantResponse 
    {
        public virtual Warrant[] Warrants { get; set; }
    }
}
