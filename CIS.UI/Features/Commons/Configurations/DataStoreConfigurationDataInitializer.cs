using CIS.Core.Entities.Commons;
using NHibernate;

namespace CIS.UI.Features.Commons.Configurations;

public class DataStoreConfigurationDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var dataStore = session.QueryOver<DataStoreConfiguration>().Cacheable().SingleOrDefault();
        if (dataStore == null)
        {
            dataStore = new DataStoreConfiguration();
            session.Save(dataStore);
        }

        dataStore.BaseUri = App.Config.ConnectToProductionEnvironment
            ? DataStoreConfiguration.ProductionBaseUri
            : DataStoreConfiguration.DevelopmentBaseUri;

        App.Data.DataStore = dataStore;

        transaction.Commit();
    }
}
