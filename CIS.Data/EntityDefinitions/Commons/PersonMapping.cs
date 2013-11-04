using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons
{
    public class PersonMapping : ComponentMap<Person>
    {
        public PersonMapping()
        {
            Map(x => x.FirstName)
                .Index("FirstNameIndex");

            Map(x => x.MiddleName)
                .Index("MiddleNameIndex");

            Map(x => x.LastName)
                .Index("LastNameIndex");

            Map(x => x.Suffix);

            Map(x => x.Gender);

            Map(x => x.BirthDate);
        }

        internal static Action<ComponentPart<Person>> Map(string columnPrefix = "")
        {
            return mapping =>
            {
                mapping.Map(x => x.FirstName, columnPrefix + "FirstName").Index("FirstNameIndex");

                mapping.Map(x => x.MiddleName, columnPrefix + "MiddleName").Index("MiddelNameIndex");

                mapping.Map(x => x.LastName, columnPrefix + "LastName").Index("LastNameIndex");

                mapping.Map(x => x.Suffix, columnPrefix + "Suffix");

                mapping.Map(x => x.Gender, columnPrefix + "Gender");

                mapping.Map(x => x.BirthDate, columnPrefix + "BirthDate");
            };
        }


        internal static Action<ComponentPart<Person>> MapBasic(string columnPrefix = "")
        {
            return mapping =>
            {
                mapping.Map(x => x.FirstName, columnPrefix + "FirstName").Index("FirstNameIndex");

                mapping.Map(x => x.MiddleName, columnPrefix + "MiddleName").Index("MiddleNameIndex");

                mapping.Map(x => x.LastName, columnPrefix + "LastName").Index("LastNameIndex");

                mapping.Map(x => x.Suffix, columnPrefix + "Suffix");
            };
        }
    }
}
