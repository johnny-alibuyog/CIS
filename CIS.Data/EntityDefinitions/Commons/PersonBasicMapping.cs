﻿using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Commons;

public class PersonBasicMapping : ComponentMap<PersonBasic>
{
    public PersonBasicMapping()
    {
        Map(x => x.Prefix);

        Map(x => x.FirstName)
            .Index("FirstNameIndex");

        Map(x => x.MiddleName)
            .Index("MiddleNameIndex");

        Map(x => x.LastName)
            .Index("LastNameIndex");

        Map(x => x.Suffix);
    }
}
