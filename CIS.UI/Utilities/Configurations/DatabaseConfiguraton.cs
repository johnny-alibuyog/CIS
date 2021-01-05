using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Memberships;

namespace CIS.UI.Utilities.Configurations
{
    [Serializable()]
    public class DatabaseConfiguraton
    {
        public virtual string ServerName { get; set; }
        public virtual string DatabaseName { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }

        public DatabaseConfiguraton()
        {
            this.ServerName = "PHMANAALVAREZ01";
            this.DatabaseName = "cisdb";
            this.Username = "sa";
            this.Password = "1234";
        }
    }
}
