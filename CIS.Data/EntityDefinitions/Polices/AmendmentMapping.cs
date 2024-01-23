using CIS.Core.Entities.Polices;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Polices;

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
