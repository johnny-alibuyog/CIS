using CIS.Core.Entities.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Commons;

public class BarcodeMapping : ClassMap<Barcode>
{
    public BarcodeMapping()
    {
        Id(x => x.Id);

        References(x => x.Image)
            .Cascade.All()
            .Fetch.Join();

        Map(x => x.Text);
    }
}
