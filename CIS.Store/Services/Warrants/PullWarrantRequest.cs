using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace CIS.Store.Services.Warrants
{
    [Route("/warrants/pull", "PATCH")]
    public class PullWarrantRequest : IReturn<PullWarrantResponse>
    {
        public virtual int FetchSize { get; set; }
        public virtual long LatestId { get; set; }
        public virtual long[] MissingIds { get; set; }
        public virtual ClientInfo Client { get; set; }
    }
}