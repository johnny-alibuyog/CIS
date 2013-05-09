using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Commons
{
    public class ImageBlobMapping : SubclassMap<ImageBlob>
    {
        public ImageBlobMapping()
        {
            DiscriminatorValue("ImageBlob");
        }
    }
}
