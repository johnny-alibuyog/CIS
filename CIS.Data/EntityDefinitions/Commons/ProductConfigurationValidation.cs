using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Commons
{
    public class ProductConfigurationValidation : ValidationDef<ProductConfiguration>
    {
        public ProductConfigurationValidation()
        {

        }
    }
}
