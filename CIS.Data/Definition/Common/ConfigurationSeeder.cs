using System;
using CIS.Core.Domain.Common;
using CIS.Core.Utility.Extention;
using NHibernate;

namespace CIS.Data.Definition.Common;

public class ConfigurationSeeder
{
    public class DataStore(ISessionFactory sessionFactory) : ISeeder
    {
        private readonly ISessionFactory _sessionFactory = sessionFactory;

        public Func<bool> IsProductionEnvironment { private get; set; }

        public Action<DataStoreConfiguration> Updated { private get; set; }

        public void Seed()
        {
            using var session = _sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();

            var dataStore = session.QueryOver<DataStoreConfiguration>().Cacheable().SingleOrDefault();
            if (dataStore == null)
            {
                dataStore = new DataStoreConfiguration();
                session.Save(dataStore);
            }

            dataStore.BaseUri = this.IsProductionEnvironment()
                ? DataStoreConfiguration.ProductionBaseUri
                : DataStoreConfiguration.DevelopmentBaseUri;

            this.Updated?.Invoke(dataStore);

            transaction.Commit();
        }
    }

    public class ImageScaleFactor(ISessionFactory sessionFactory) : ISeeder
    {
        private readonly ISessionFactory _sessionFactory = sessionFactory;

        public Action<ImageScaleFactorConfiguration> Updated { private get; set; }

        public void Seed()
        {
            using var session = _sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();

            var configuration = session.QueryOver<ImageScaleFactorConfiguration>().Cacheable().SingleOrDefault() ?? new();

            session.SaveOrUpdate(configuration);

            this.Updated?.Invoke(configuration);

            transaction.Commit();
        }
    }

    public class Product(ISessionFactory sessionFactory) : ISeeder
    {
        private readonly ISessionFactory _sessionFactory = sessionFactory;

        public Action<ProductConfiguration> Updated { private get; set; }

        public void Seed()
        {
            using var session = _sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();

            var product = session.QueryOver<ProductConfiguration>().Cacheable().SingleOrDefault() ?? new();

            session.SaveOrUpdate(product);

            this.Updated?.Invoke(product);

            transaction.Commit();
        }
    }

    public class ProperCasing(ISessionFactory sessionFactory) : ISeeder
    {
        private readonly ISessionFactory _sessionFactory = sessionFactory;

        public Action<ImageScaleFactorConfiguration> Updated { private get; set; }

        public void Seed()
        {
            using var session = _sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();

            var properCasing = session.QueryOver<ProperCasingConfiguration>().Cacheable().SingleOrDefault() ?? new();

            StringExtention.SetProperCaseSuffix(properCasing.Suffixes);
            StringExtention.SetProperCaseSpecialWords(properCasing.SpecialWords);

            properCasing.IsProperCasingInitialized = true;

            session.SaveOrUpdate(properCasing);

            transaction.Commit();
        }
    }


}
