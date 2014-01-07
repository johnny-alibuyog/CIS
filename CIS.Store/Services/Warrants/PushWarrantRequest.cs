using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CIS.Store.Domain.Entities;
using ServiceStack;

namespace CIS.Store.Services.Warrants
{
    [Route("/warrants/push", "PATCH")]
    public class PushWarrantRequest : IReturn<PushWarrantResponse>
    {
        public virtual int FetchSize { get; set; }
        public virtual Warrant[] Warrants { get; set; }
    }
}