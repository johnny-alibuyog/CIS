using CIS.Core.Entities.Barangays;
using CIS.Data.Commons.Extentions;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinition.Barangays;

public class ClearanceMapping : ClassMap<Clearance>
{
    public ClearanceMapping()
    {
        OptimisticLock.Version();

        Id(x => x.Id);

        Version(x => x.Version);

        Component(x => x.Audit);

        References(x => x.Applicant)
            .Cascade.SaveUpdate();

        References(x => x.ApplicantPicture);

        References(x => x.ApplicantSignature);

        Map(x => x.ApplicantAddress);

        References(x => x.Office);

        HasManyToMany(x => x.Officials)
            .Access.CamelCaseField(Prefix.Underscore)
            .Schema(GetType().ParseSchema())
            .Table("ClearancesOfficials")
            .Cascade.SaveUpdate()
            .AsSet();

        Map(x => x.ApplicationDate);

        Map(x => x.IssueDate);

        Map(x => x.Fee);

        Map(x => x.ControlNumber);

        Map(x => x.OfficialReceiptNumber);

        Map(x => x.TaxCertificateNumber);

        Map(x => x.FinalFindings);

        References(x => x.Finding)
            .Cascade.All();

        References(x => x.Purpose);
    }
}
