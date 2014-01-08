using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Commons
{
    public class ImageScaleFactorConfigurationMapping : SubclassMap<ImageScaleFactorConfiguration>
    {
        public ImageScaleFactorConfigurationMapping()
        {
            DiscriminatorValue("ImageScaleFactorConfiguration");
        }
    }
}
