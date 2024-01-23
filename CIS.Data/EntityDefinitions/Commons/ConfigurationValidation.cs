using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinitions.Commons;

public class ConfigurationValidation : ValidationDef<Configuration>
{
    public ConfigurationValidation()
    {
        Define(x => x.Id);

        Define(x => x.Properties);
    }
}
