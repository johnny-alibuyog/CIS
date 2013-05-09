using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.Mappings.Commons
{
    public class BarcodeMapping : ClassMap<Barcode>
    {
        public BarcodeMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            References(x => x.Image)
                .Cascade.All()
                .Fetch.Join();

            Map(x => x.Text);
        }
    }
}
