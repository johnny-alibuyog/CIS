﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Westwind.Utilities.Configuration;

namespace CIS.UI.Utilities.Configurations
{
    [Serializable()]
    public class ApplicationConfiguration : AppConfiguration
    {
        public virtual AppearanceConfiguration Apprearance { get; set; }
        public virtual string MailserverPassword { get; set; }
        public virtual string ConnectionString { get; set; }

        public ApplicationConfiguration()
        {
            this.Apprearance = new AppearanceConfiguration();
            this.MailserverPassword = "12345a#";
            this.ConnectionString = "connection string #";

            this.Provider = new XmlFileConfigurationProvider<ApplicationConfiguration>()
            {
                PropertiesToEncrypt = "MailserverPassword,ConnectionString",
                EncryptionKey = "_secretive_",
                XmlConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationConfiguration.config")
            };

            this.Provider.Read(this);
        }
    }
}
