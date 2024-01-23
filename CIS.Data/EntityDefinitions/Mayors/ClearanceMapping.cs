using CIS.Core.Entities.Mayors;
using CIS.Data.EntityDefinition.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Mayors;

public class ClearanceMapping : ClassMap<Clearance>
{
    public ClearanceMapping()
    {
        OptimisticLock.Version();

        Id(x => x.Id);

        Version(x => x.Version);

        Component(x => x.Audit);

        Component(x => x.Applicant, PersonMapping.MapBasic("Applicant"));

        Component(x => x.ApplicantAddress)
            .ColumnPrefix("Applicant");

        References(x => x.ApplicantPicture)
            .Cascade.All();

        Map(x => x.Company);

        Component(x => x.CompanyAddress)
            .ColumnPrefix("Company");

        Component(x => x.Mayor, PersonMapping.MapBasic("Mayor"));

        Component(x => x.Secretary, PersonMapping.MapBasic("Secretary"));

        Map(x => x.IssueDate);

        Map(x => x.PermitNumber);

        Map(x => x.PermitFee);

        Map(x => x.Penalty);

        Map(x => x.OfficialReceiptNumber);

        Map(x => x.Notice);
    }
}
