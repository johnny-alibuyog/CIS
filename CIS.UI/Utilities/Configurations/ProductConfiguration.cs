using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Utilities.Configurations
{
    [Serializable()]
    public class ProductConfiguration
    {
        public virtual string Licensee { get; set; }
        public virtual string AboutTemplate { get; set; }
        public virtual string[] Plugins { get; set; }

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
            this.Plugins = new[] 
            { 
                "NHibernate", 
                "Ninject", 
                "ReactiveUI" 
            };
        }
    }
}
