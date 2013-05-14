using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons
{
    public class EmailMapping : SubclassMap<Email>
    {
        public EmailMapping()
        {
            DiscriminatorValue("Email");
        }
    }
}
