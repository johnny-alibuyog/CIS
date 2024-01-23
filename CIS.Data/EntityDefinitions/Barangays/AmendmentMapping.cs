using CIS.Core.Entities.Barangays;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Barangays;

public class AmendmentMapping : ClassMap<Amendment>
{
    public AmendmentMapping()
    {
        Id(x => x.Id);

        References(x => x.Approver);

        Map(x => x.DocumentNumber);

        Map(x => x.Reason);

        Map(x => x.Remarks);
    }
}
