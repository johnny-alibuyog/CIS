using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using NHibernate;

namespace CIS.UI.Features.Commons.Configurations
{
    public class DataStoreConfigurationDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public DataStoreConfigurationDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var dataStore = session.QueryOver<DataStoreConfiguration>().Cacheable().SingleOrDefault();
                if (dataStore == null)
                {
                    dataStore = new DataStoreConfiguration();
                    session.Save(dataStore);
                }

                App.Data.DataStore = dataStore;

                transaction.Commit();
            }
        }
    }
}
