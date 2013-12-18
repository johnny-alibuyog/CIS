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
        public virtual string ApplicationDataLocation { get; set; }
        public virtual LoginConfiguration Login { get; set; }
        public virtual ProductConfiguration Product { get; set; }
        public virtual DatabaseConfiguraton Database { get; set; }
        public virtual DataStoreConfiguration DataStore { get; set; }
        public virtual AppearanceConfiguration Apprearance { get; set; }
        public virtual ProperCasingConfiguration ProperCasing { get; set; }
        public virtual ImageScaleFactorConfiguration Image { get; set; }

        public ApplicationConfiguration()
        {
            this.ApplicationDataLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CIS");
            if (Directory.Exists(this.ApplicationDataLocation) == false)
                Directory.CreateDirectory(this.ApplicationDataLocation);

            this.Login = new LoginConfiguration();
            this.Product = new ProductConfiguration();
            this.Database = new DatabaseConfiguraton();
            this.DataStore = new DataStoreConfiguration();
            this.Apprearance = new AppearanceConfiguration();
            this.ProperCasing = new ProperCasingConfiguration();
            this.Image = new ImageScaleFactorConfiguration();
        }

        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            return new XmlFileConfigurationProvider<ApplicationConfiguration>()
            {
                PropertiesToEncrypt = "MailserverPassword,ConnectionString",
                EncryptionKey = "_secretive_",
                XmlConfigurationFile = Path.Combine(this.ApplicationDataLocation, "cis.config")
            };
        }
    }
}
