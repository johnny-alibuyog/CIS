using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CIS.Data.EntityDefinition.Commons;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;

namespace CIS.Data.Configurations
{
    internal static class ValidatorConfiguration
    {
        private static readonly ValidatorEngine _validatorEnine = new ValidatorEngine();

        public static ValidatorEngine ValidatorEngine
        {
            get { return _validatorEnine; }
        }

        public static void Configure(this Configuration configuration)
        {
            var validatorEngine = GetValidatorEngine();
            new ValidatorSharedEngineProvider(validatorEngine).UseMe();
            configuration.Initialize(validatorEngine);
        }

        private static ValidatorEngine GetValidatorEngine()
        {
            var configuration = GetConfiguration();
            _validatorEnine.Configure(configuration);
            return _validatorEnine;
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
}
