using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class EducationDefinition
{
    public class Mapping : ClassMap<Education>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Level);

            Map(x => x.SchoolName);

            Map(x => x.YearGraduated);
        }
    }

    public class Validation : ValidationDef<Education>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.SchoolName)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.YearGraduated);
        }
    }
}
