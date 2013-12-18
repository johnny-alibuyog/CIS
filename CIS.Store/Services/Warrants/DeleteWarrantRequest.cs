using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace CIS.Store.Services.Warrants
{
    [Route("/warrants/delete/{Id}", "DELETE")]
    public class DeleteWarrantRequest : IReturn<DeleteWarrantResponse>
    {
        public virtual long Id { get; set; }
    }
}