using CIS.Core.Domain.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Definition.Common;

public class SkillDefinition
{
    public class Mapping : ClassMap<Skill>
    {
        public Mapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name);
        }
    }

    public class Validation : ValidationDef<Skill>
    {
        public Validation()
        {
            Define(x => x.Id);
    
            Define(x => x.Name)
                .MaxLength(150);
        }
    }
}
