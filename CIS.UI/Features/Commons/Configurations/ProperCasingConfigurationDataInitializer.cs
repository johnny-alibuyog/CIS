using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;
using NHibernate;

namespace CIS.UI.Features.Commons.Configurations;

public class ProperCasingConfigurationDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var properCasing = session.QueryOver<ProperCasingConfiguration>().Cacheable().SingleOrDefault();
        if (properCasing == null)
        {
            properCasing = new ProperCasingConfiguration();
            session.Save(properCasing);
        }

        StringExtention.SetProperCaseSuffix(properCasing.Suffixes);
        StringExtention.SetProperCaseSpecialWords(properCasing.SpecialWords);

        transaction.Commit();
    }

    public virtual void SetProperCasingIsInitialized(bool p)
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var configuration = session.QueryOver<ProperCasingConfiguration>().Cacheable().SingleOrDefault();
        if (configuration == null)
        {
            configuration = new ProperCasingConfiguration();
            session.Save(configuration);
        }
        configuration.IsProperCasingInitialized = true;

        transaction.Commit();
    }
}
