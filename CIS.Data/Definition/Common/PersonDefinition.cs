using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;
using System;

namespace CIS.Data.Definition.Common;

public class PersonDefinition
{
    public class Mapping : ComponentMap<Person>
    {
        public Mapping()
        {
            Map(x => x.Prefix);

            Map(x => x.FirstName)
                .Index("FirstNameIndex");

            Map(x => x.MiddleName)
                .Index("MiddleNameIndex");

            Map(x => x.LastName)
                .Index("LastNameIndex");

            Map(x => x.Suffix);

            Map(x => x.Gender);

            Map(x => x.BirthDate);

            Map(x => x.BirthPlace);
        }

        internal static Action<ComponentPart<Person>> Map(string columnPrefix = "")
        {
            return mapping =>
            {
                mapping.Map(x => x.Prefix, columnPrefix + "Prefix");

                mapping.Map(x => x.FirstName, columnPrefix + "FirstName").Index("FirstNameIndex");

                mapping.Map(x => x.MiddleName, columnPrefix + "MiddleName").Index("MiddelNameIndex");

                mapping.Map(x => x.LastName, columnPrefix + "LastName").Index("LastNameIndex");

                mapping.Map(x => x.Suffix, columnPrefix + "Suffix");

                mapping.Map(x => x.Gender, columnPrefix + "Gender");

                mapping.Map(x => x.BirthDate, columnPrefix + "BirthDate");

                mapping.Map(x => x.BirthPlace, columnPrefix + "BirthPlace");
            };
        }


        internal static Action<ComponentPart<Person>> MapBasic(string columnPrefix = "")
        {
            return mapping =>
            {
                mapping.Map(x => x.Prefix, columnPrefix + "Prefix");

                mapping.Map(x => x.FirstName, columnPrefix + "FirstName").Index("FirstNameIndex");

                mapping.Map(x => x.MiddleName, columnPrefix + "MiddleName").Index("MiddleNameIndex");

                mapping.Map(x => x.LastName, columnPrefix + "LastName").Index("LastNameIndex");

                mapping.Map(x => x.Suffix, columnPrefix + "Suffix");
            };
        }

        internal static Action<CompositeElementPart<Person>> MapBasicCollection()
        {
            return mapping =>
            {
                mapping.Map(x => x.Prefix);

                mapping.Map(x => x.FirstName).Index("FirstNameIndex");

                mapping.Map(x => x.MiddleName).Index("MiddleNameIndex");

                mapping.Map(x => x.LastName).Index("LastNameIndex");

                mapping.Map(x => x.Suffix);
            };
        }

    }

    public class Validation : ValidationDef<Person>
    {
        public Validation()
        {
            Define(x => x.Prefix)
                .MaxLength(150);

            Define(x => x.FirstName)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.MiddleName)
                .MaxLength(150);

            Define(x => x.LastName)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Suffix)
                .MaxLength(150);

            Define(x => x.Gender);

            Define(x => x.BirthDate)
                .IsInThePast();

            Define(x => x.BirthPlace)
                .MaxLength(150);
        }
    }
}
