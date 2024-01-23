using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Barangays;

public class OfficeMapping : ClassMap<Office>
{
    public OfficeMapping()
    {
        OptimisticLock.Version();

        Id(x => x.Id);

        Version(x => x.Version);

        Component(x => x.Audit);

        References(x => x.Logo)
            .Cascade.All()
            .Fetch.Join();

        Map(x => x.Name);

        Map(x => x.Location);

        Component(x => x.Address);

        Map(x => x.ClearanceFee);

        Map(x => x.CertificationFee);

        Map(x => x.DocumentStampTax);

        HasMany(x => x.Incumbents)
            .Access.CamelCaseField(Prefix.Underscore)
            .Cascade.AllDeleteOrphan()
            .Not.KeyNullable()
            .Not.KeyUpdate()
            .Inverse()
            .AsSet();
    }
}
