using CIS.Core.Domain.Membership;
using CIS.Data.Common.Extention;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Membership;

public class ApplicationDefinition
{
    public class Mapping : ClassMap<Application>
    {
        public Mapping()
        {
            Id(x => x.Id);

            Component(x => x.Person);

            Component(x => x.Father)
                .ColumnPrefix("Father");

            Component(x => x.Mother)
                .ColumnPrefix("Mother");

            HasMany(x => x.Registrations)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();

            HasMany(x => x.Relatives)
                .Schema(GetType().ParseSchema())
                .Table("ApplicantsRelatives")
                .KeyColumn("ApplicantsRelativesId")
                //.Component(x => PersonMapping.MapBasicCollection())
                .Component(component =>
                {
                    component.Map(x => x.Prefix)
                        .Length(150);

                    component.Map(x => x.FirstName)
                        .Index("FirstNameIndex")
                        .Length(150)
                        .Nullable();

                    component.Map(x => x.MiddleName)
                        .Index("MiddleNameIndex")
                        .Length(150)
                        .Nullable();

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

            HasManyToMany(x => x.Pictures)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("ApplicantsPictures")
                .Cascade.AllDeleteOrphan()
                .AsSet();

            HasManyToMany(x => x.Signatures)
                .Access.CamelCaseField(Prefix.Underscore)
                .Schema(GetType().ParseSchema())
                .Table("ApplicantsSignatures")
                .Cascade.AllDeleteOrphan()
                .AsSet();

            References(x => x.FingerPrint)
                .Cascade.All();

            Map(x => x.Height);

            Map(x => x.Weight);

            Map(x => x.Build);

            Map(x => x.Marks);

            Map(x => x.AlsoKnownAs);

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

    public class Validation : ValidationDef<Application>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Mother)
                .IsValid();

            Define(x => x.Father)
                .IsValid();

            Define(x => x.Registrations)
                .HasValidElements();

            Define(x => x.Relatives)
                .HasValidElements();

            Define(x => x.Address)
                .NotNullable()
                .And.IsValid();

            Define(x => x.ProvincialAddress)
                .IsValid();

            Define(x => x.Pictures);

            Define(x => x.Signatures);

            Define(x => x.FingerPrint);

            Define(x => x.Height)
                .NotNullableAndNotEmpty()
                .And.MaxLength(20);

            Define(x => x.Weight)
                .NotNullableAndNotEmpty()
                .And.MaxLength(20);

            Define(x => x.Build)
                .MaxLength(150);

            Define(x => x.Marks)
                .MaxLength(150);

            Define(x => x.AlsoKnownAs)
                .MaxLength(150);

            Define(x => x.Occupation)
                .MaxLength(150);

            Define(x => x.Religion)
                .MaxLength(150);

            Define(x => x.Citizenship)
                .MaxLength(100);

            Define(x => x.EmailAddress)
                .MaxLength(100);

            Define(x => x.TelephoneNumber)
                .MaxLength(100);

            Define(x => x.CellphoneNumber)
                .MaxLength(100);

            Define(x => x.PassportNumber)
                .MaxLength(50);

            Define(x => x.TaxIdentificationNumber)
                .MaxLength(50);

            Define(x => x.SocialSecuritySystemNumber)
                .MaxLength(50);

            Define(x => x.CivilStatus);
        }
    }
}
