using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Utilities.Configurations
{
    [Serializable()]
    public class DataStoreConfiguration
    {
        public virtual string BaseUri { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual bool Syncronize { get; set; }
        public int SyncronizeInterval { get; set; }
        public virtual int FetchSize { get; set; }

        public DataStoreConfiguration()
        {
            this.BaseUri = "http://localhost:53890/";
            //this.BaseUri = "http://cisstore.azurewebsites.net";
            this.Username = "cistoreuser";
            this.Password = "123456a$";
            this.Syncronize = true;
            this.SyncronizeInterval = 60;
            this.FetchSize = 100;
        }
    }
}
