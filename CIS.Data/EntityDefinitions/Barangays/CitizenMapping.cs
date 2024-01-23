using CIS.Core.Entities.Barangays;
using CIS.Data.Commons.Extentions;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Barangays;

public class CitizenMapping : ClassMap<Citizen>
{
    public CitizenMapping()
    {
        OptimisticLock.Version();

        Id(x => x.Id);

        Version(x => x.Version);

        Component(x => x.Audit);

        Component(x => x.Person);

        Map(x => x.CivilStatus);

        Map(x => x.AlsoKnownAs);

        Map(x => x.BirthPlace);

        Map(x => x.Occupation);

        Map(x => x.Religion);

        Map(x => x.Citizenship);

        Map(x => x.EmailAddress);

        Map(x => x.TelephoneNumber);

        Map(x => x.CellphoneNumber);

        Component(x => x.CurrentAddress)
            .ColumnPrefix("Current");

        Component(x => x.ProvincialAddress)
            .ColumnPrefix("Province");

        References(x => x.FingerPrint)
            .Cascade.All();

        HasManyToMany(x => x.Pictures)
            .Access.CamelCaseField(Prefix.Underscore)
            .Schema(GetType().ParseSchema())
            .Table("CitizensPictures")
            .Cascade.AllDeleteOrphan()
            .AsSet();

        HasManyToMany(x => x.Signatures)
            .Access.CamelCaseField(Prefix.Underscore)
            .Schema(GetType().ParseSchema())
            .Table("CitizensSignatures")
            .Cascade.AllDeleteOrphan()
            .AsSet();
    }
}
