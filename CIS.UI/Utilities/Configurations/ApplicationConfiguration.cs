using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Memberships;
using Westwind.Utilities.Configuration;

namespace CIS.UI.Utilities.Configurations
{
    [Serializable()]
    public class ApplicationConfiguration : AppConfiguration
    {
        public virtual string Licensee { get; set; }
        public virtual string[] Plugins { get; set; }
        public virtual string ConnectionString { get; set; }
        public virtual string MailserverPassword { get; set; }
        public virtual string PowerUser { get; set; }
        public virtual bool UserPowerUser { get; set; }
        public virtual DatabaseConfiguraton Database { get; set; }
        public virtual AppearanceConfiguration Apprearance { get; set; }

        public ApplicationConfiguration()
        {
            this.Licensee = "JLRC Manasoft";
            this.Plugins = new[] 
            { 
                "NHibernate", 
                "Ninject", 
                "ReactiveUI" 
            };

            this.ConnectionString = "connection string #";
            this.MailserverPassword = "12345a#";

            this.PowerUser = "power_admin";
            this.UserPowerUser = true;

            this.Database = new DatabaseConfiguraton()
            {
                ServerName = "(local)",
                DatabaseName = "cisdb",
                Username = "sa",
                Password = "admin123"

            };

            this.Apprearance = new AppearanceConfiguration()
            {
                Color = new AppearanceColorConfiguration(),
                Theme = new AppearanceThemeConfiguration()
            };

            this.Provider = new XmlFileConfigurationProvider<ApplicationConfiguration>()
            {
                //PropertiesToEncrypt = "MailserverPassword,ConnectionString,Database.Username,Database.Password",
                PropertiesToEncrypt = "MailserverPassword,ConnectionString",
                EncryptionKey = "_secretive_",
                XmlConfigurationFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "cis.config")
            };

            this.Provider.Read(this);
            //this.Provider.Write(this);
        }
    }
}
