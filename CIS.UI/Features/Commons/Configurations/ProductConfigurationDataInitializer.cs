using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Commons.Configurations
{
    public class ProductConfigurationDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public ProductConfigurationDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;

        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var configuration = session.QueryOver<ProductConfiguration>().Cacheable().SingleOrDefault();
                if (configuration == null)
                {
                    configuration = new ProductConfiguration();
                    session.Save(configuration);
                }

                App.Data.Product = configuration;

                transaction.Commit();
            }
        }
    }
}
