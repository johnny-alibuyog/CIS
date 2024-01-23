using CIS.Core.Entities.Commons;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace CIS.UI.Features.Commons.Biometrics;

public class FingerDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var fingers = session.Query<Finger>().Cacheable().ToList();

        foreach (var item in Finger.All)
        {
            if (fingers.Contains(item))
                continue;

            session.Save(item);
        }

        transaction.Commit();
    }
}
