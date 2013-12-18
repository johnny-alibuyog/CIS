using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CIS.Store.Domain.Entities;

namespace CIS.Store.Services.Warrants
{
    public class GetWarrantResponse
    {
        public virtual Warrant[] Warrants { get; set; }
    }
}