using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Utilities.Configurations
{
    [Serializable()]
    public class DatabaseConfiguraton
    {
        private string _serverName;
        private string _databaseName;
        private string _username;
        private string _password;

        public virtual string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }

        public virtual string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }

        public virtual string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public virtual string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public DatabaseConfiguraton()
        {
            _serverName = "(local)";
            _databaseName = "cisdb";
            _username = "sa";
            _password = "admin123";
        }
    }
}
