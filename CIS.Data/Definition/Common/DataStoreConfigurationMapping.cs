using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;

namespace CIS.Data.Definition.Common
{
    public class DataStoreConfigurationMapping : SubclassMap<DataStoreConfiguration>
    {
        public DataStoreConfigurationMapping()
        {
            DiscriminatorValue("DataStoreConfiguration");
        }
    }
}
