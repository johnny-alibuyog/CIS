﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Commons
{
    public class DataStoreConfiguration : Configuration
    {
        public static readonly string ProductionBaseUri = "http://cisstore.azurewebsites.net";
        public static readonly string DevelopmentBaseUri = "http://localhost:53890/";

        public virtual string BaseUri
        {
            get { return this.GetPropertyValue("BaseUri"); }
            set { this.SetPropertyValue("BaseUri", value.ToString()); }
        }

        public virtual string Username
        {
            get { return this.GetPropertyValue("Username"); }
            set { this.SetPropertyValue("Username", value.ToString()); }
        }

        public virtual string Password
        {
            get { return this.GetPropertyValue("Password"); }
            set { this.SetPropertyValue("Password", value.ToString()); }
        }

        public virtual bool Syncronize
        {
            get { return bool.Parse(this.GetPropertyValue("Syncronize")); }
            set { this.SetPropertyValue("Syncronize", value.ToString()); }
        }

        public virtual int SyncronizeInterval
        {
            get { return int.Parse(this.GetPropertyValue("SyncronizeInterval")); }
            set { this.SetPropertyValue("SyncronizeInterval", value.ToString()); }
        }

        public virtual int FetchSize
        {
            get { return int.Parse(this.GetPropertyValue("FetchSize")); }
            set { this.SetPropertyValue("FetchSize", value.ToString()); }
        }

        public DataStoreConfiguration()
        {
            this.BaseUri = DevelopmentBaseUri;
            this.Username = "cisstoredbuser";
            this.Password = "!@#123qwe";
            this.Syncronize = true;
            this.SyncronizeInterval = 60;
            this.FetchSize = 100;
        }
    }
}
