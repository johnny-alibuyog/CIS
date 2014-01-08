using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CIS.Store.Domain.Entities;
using ServiceStack;

namespace CIS.Store.Services.Warrants
{
    [Route("/warrants", "POST")]
    public class InsertWarrantRequest : IReturn<InsertWarrantResponse>
    {
        public virtual Warrant Warrant { get; set; }
        public virtual ClientInfo Client { get; set; }
    }
}