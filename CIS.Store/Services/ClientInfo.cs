using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS.Store.Services
{
    public class ClientInfo
    {
        public virtual string Username { get; set; }
        public virtual string Origin { get; set; }

        public ClientInfo() { }

        public ClientInfo(string username, string origin)
        {
            this.Username = username;
            this.Origin = origin;
        }
    }
}