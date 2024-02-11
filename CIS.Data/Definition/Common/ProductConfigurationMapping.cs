using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;

namespace CIS.Data.Definition.Common
{
    public class ProductConfigurationMapping : SubclassMap<ProductConfiguration>
    {
        public ProductConfigurationMapping()
        {
            DiscriminatorValue("ProductConfiguration");
        }
    }
}
