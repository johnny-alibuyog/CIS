using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace CIS.Store.Services.Warrants
{
    [Route("/warrants", "GET")]
    [Route("/warrants/{Id}", "GET")]
    public class GetWarrantRequest : IReturn<GetWarrantResponse>
    {
        public virtual long Id { get; set; }
        public virtual ClientInfo Client { get; set; }
    }
}