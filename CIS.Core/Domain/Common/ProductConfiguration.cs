using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Domain.Common
{
    public class ProductConfiguration : Configuration
    {
        public virtual string Licensee
        {
            get { return this.GetPropertyValue("Licensee"); }
            set { this.SetPropertyValue("Licensee", value); }
        }

        public virtual string AboutTemplate
        {
            get { return this.GetPropertyValue("AboutTemplate"); }
            set { this.SetPropertyValue("AboutTemplate", value); }
        }

        public virtual IEnumerable<string> Plugins
        {
            get { return this.GetListPropertyValue("Plugins"); }
            set { this.SetListPropertyValue("Plugins", value); }
        }

        public ProductConfiguration()
        {
            this.Licensee = "JLRC IT Solutions";
            this.AboutTemplate += Environment.NewLine;
            this.AboutTemplate += "[b]LICENSEE[/b] is commitment to deliver innovative, powerful and first-rate products and services.";
            this.AboutTemplate += Environment.NewLine + Environment.NewLine;
            this.AboutTemplate += "An award-winning global developer and provider of intelligent IT Solutions offering a broad range of expertise in software development, systems integration and collaboration as well as software and hardware sales.";
            this.AboutTemplate += Environment.NewLine + Environment.NewLine;
            this.AboutTemplate += "At the forefront of software development. Constantly developing cost effective and reliable application to meet the client's ever changing needs. Whether developing specialized applications from ground up or modifying existing programs or integrating systems, Manasoft has the technical skills to design, develop, test and implement applications on a broad range of platforms.";
            this.AboutTemplate += Environment.NewLine + Environment.NewLine;
            this.Plugins = new List<string>() { "NHibernate", "Ninject", "ReactiveUI" };

        }
    }
}
