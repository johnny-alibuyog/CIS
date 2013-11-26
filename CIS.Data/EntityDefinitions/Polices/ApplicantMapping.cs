using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.Data.Commons.Extentions;
using CIS.Data.EntityDefinition.Commons;
using FluentNHibernate.Mapping;

namespace CIS.Data.EntityDefinitions.Polices
{
    public class ApplicantMapping : ClassMap<Applicant>
    {
        public ApplicantMapping()
        {
            Id(x => x.Id);

            Component(x => x.Person);

            Component(x => x.Father, PersonMapping.MapBasic("Father"));

            Component(x => x.Mother, PersonMapping.MapBasic("Mother"));

            HasMany<Person>(x => x.Relatives)
                .Schema(GetType().ParseSchema())
                .Table("ApplicantsRelatives")
                .KeyColumn("ApplicantsRelativesId")
                //.Component(x => PersonMapping.MapBasicCollection())
                .Component(component =>
                {
                    component.Map(x => x.FirstName)
                        .Index("FirstNameIndex")
                        .Length(150)
                        .Nullable();

                    component.Map(x => x.MiddleName)
                        .Index("MiddleNameIndex")
                        .Length(150);

                    component.Map(x => x.LastName)
                        .Index("LastNameIndex")
                        .Length(150)
                        .Nullable();

                    component.Map(x => x.Suffix)
                        .Length(150);
                })
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan();
                //.Not.KeyNullable()
                //.Not.KeyUpdate()
                //.Inverse();

            Component(x => x.Address);

            Component(x => x.ProvincialAddress)
                .ColumnPrefix("Province");

            References(x => x.Picture)
                .Cascade.All();

            References(x => x.Signature)
                .Cascade.All();

            References(x => x.FingerPrint)
                .Cascade.All();

            Map(x => x.Height);

            Map(x => x.Weight);

            Map(x => x.Build);

            Map(x => x.Marks);

            Map(x => x.AlsoKnownAs);

            Map(x => x.BirthPlace);

            Map(x => x.Occupation);

            Map(x => x.Religion);

            Map(x => x.Citizenship);

            Map(x => x.EmailAddress);

            Map(x => x.TelephoneNumber);

            Map(x => x.CellphoneNumber);

            Map(x => x.PassportNumber);

            Map(x => x.TaxIdentificationNumber);

            Map(x => x.SocialSecuritySystemNumber);

            Map(x => x.CivilStatus);
        }
    }
}
