using CIS.Data.EntityDefinition.Commons;
using NHibernate.Cfg;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using System.Reflection;

namespace CIS.Data.Configurations;

internal static class ValidatorConfiguration
{
    public static void Configure(this Configuration configuration)
    {
        var validatorEngine = GetValidatorEngine();
        new ValidatorSharedEngineProvider(validatorEngine).UseMe();
        configuration.Initialize(validatorEngine);
    }

    private static ValidatorEngine GetValidatorEngine()
    {
        var configuration = GetConfiguration();
        SessionProvider.Validator.Configure(configuration);
        return SessionProvider.Validator;
    }

    private static FluentConfiguration GetConfiguration()
    {
        var configuration = new FluentConfiguration();
        configuration
            .SetMessageInterpolator<ConventionMessageInterpolator>()
            .SetCustomResourceManager("CIS.Data.Properties.CustomValidatorMessages", Assembly.Load("CIS.Data"))
            .SetDefaultValidatorMode(ValidatorMode.OverrideExternalWithAttribute)
            .Register(Assembly.Load(typeof(AuditValidation).Assembly.FullName).ValidationDefinitions())
            .IntegrateWithNHibernate
                .ApplyingDDLConstraints()
                .And.RegisteringListeners();

        return configuration;
    }
}
