using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class HobbyDefinition
{
    public class Mapping : ClassMap<Hobby>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name);
        }
    }

    public class Validation : ValidationDef<Hobby>
    {
        public Validation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .MaxLength(150);
        }
    }   
}
