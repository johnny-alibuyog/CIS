using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Commons
{
    public class PersonMapping : ComponentMap<Person>
    {
        public PersonMapping()
        {
            Map(x => x.FirstName);

            Map(x => x.MiddleName);

            Map(x => x.LastName);

            Map(x => x.Suffix);

            Map(x => x.Gender);

            Map(x => x.BirthDate);
        }

        internal static Action<ComponentPart<Person>> Map(string columnPrefix = "")
        {
            return mapping =>
            {
                mapping.Map(x => x.FirstName, columnPrefix + "FirstName");

                mapping.Map(x => x.MiddleName, columnPrefix + "MiddleName");

                mapping.Map(x => x.LastName, columnPrefix + "LastName");

                mapping.Map(x => x.Suffix, columnPrefix + "Suffix");

                mapping.Map(x => x.Gender, columnPrefix + "Gender");

                mapping.Map(x => x.BirthDate, columnPrefix + "BirthDate");
            };
        }


        internal static Action<ComponentPart<Person>> MapBasic(string columnPrefix = "")
        {
            return mapping =>
            {
                mapping.Map(x => x.FirstName, columnPrefix + "FirstName");

                mapping.Map(x => x.MiddleName, columnPrefix + "MiddleName");

                mapping.Map(x => x.LastName, columnPrefix + "LastName");

                mapping.Map(x => x.Suffix, columnPrefix + "Suffix");
            };
        }
    }
}
