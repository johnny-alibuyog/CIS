using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class FingerDefinition
{
    public class Mapping : ClassMap<Finger>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name);

            Map(x => x.ImageUri);
        }
    }

    public class Validation : ValidationDef<Finger>
    {
        public Validation()
        {
            Define(x => x.Id)
                .NotNullableAndNotEmpty()
                .And.MaxLength(2);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(25);

            Define(x => x.ImageUri)
                .NotNullableAndNotEmpty()
                .And.MaxLength(300);
        }
    }
}
