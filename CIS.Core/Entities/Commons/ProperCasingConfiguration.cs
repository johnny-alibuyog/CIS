using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.Core.Entities.Commons
{
    public class ProperCasingConfiguration : Configuration
    {
        public virtual bool IsProperCasingInitialized
        {
            get { return bool.Parse(this.GetPropertyValue("IsProperCasingInitialized")); }
            set { this.SetPropertyValue("IsProperCasingInitialized", value.ToString()); }
        }

        public virtual IEnumerable<string> Suffixes
        {
            get { return this.GetListPropertyValue("Suffixes"); }
            set { this.SetListPropertyValue("Suffixes", value); }
        }

        public virtual IEnumerable<string> SpecialWords
        {
            get { return this.GetListPropertyValue("SpecialWords"); }
            set { this.SetListPropertyValue("SpecialWords", value); }
        }

        public ProperCasingConfiguration()
        {
            this.IsProperCasingInitialized = false;

            this.Suffixes = new List<string>()
            {
                //"'",      // D'Artagnon, D'Silva
                ".",        //
                "-",        // Oscar-Meyer-Weiner
                "(",        //
                ")",        //
                //"Mc",     // McLaren
                //"Mac",    // MacKinly
            };

            this.SpecialWords = new List<string>()
            {
                // names words
                "dela",     // dela Cruz
                "del",      // del Rosario
                "de",       // de Guzman
                "van",      // Dick van Dyke
                "von",      // Baron von Bruin-Valt
                "di",
                "da",       // Leonardo da Vinci, Eduardo da Silva
                "of",       // The Grand Old Duke of York
                "the",      // William the Conqueror
                "HRH",      // His/Her Royal Highness
                "HRM",      // His/Her Royal Majesty
                "H.R.H.",   // His/Her Royal Highness
                "H.R.M.",   // His/Her Royal Majesty

                // Clearance Purpose words
                "AFP",      // 
                "BDO",      //
                "BIR",      // 
                "BPI",      // 
                "NBP",      // 
                "PNP",      // 
                "BDO",      // 
                "DFA",      // 
                "DOH",      // 
                "NSO",      // 
                "OJT",      // 
                "PDAE",     // 
                "PNB",      // 
                "PTC",      // 
                "SSS",      // 

                // Gun Make
                "CMMG",     // 
                "CZ",       // 
                "DPMS",     // 
                "FNH",      // 
                "SKS",      // 
            };
        }
    }
}
