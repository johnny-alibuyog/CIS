using CIS.Core.Entities.Commons;
using NHibernate;

namespace CIS.UI.Features.Commons.Configurations;

public class ProductConfigurationDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var product = session.QueryOver<ProductConfiguration>().Cacheable().SingleOrDefault();
        if (product == null)
        {
            product = new ProductConfiguration();
            session.Save(product);
        }

        App.Data.Product = product;

        transaction.Commit();
    }
}
