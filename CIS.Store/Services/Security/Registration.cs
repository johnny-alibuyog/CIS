using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace CIS.Store.Services.Security
{
    [Authenticate()]
    public class Registration : Service
    {

    }
}
