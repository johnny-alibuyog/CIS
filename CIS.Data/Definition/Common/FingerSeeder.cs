using System.Linq;
using CIS.Core.Domain.Common;
using NHibernate;
using NHibernate.Linq;

namespace CIS.Data.Definition.Common;

public class FingerSeeder(ISessionFactory sessionFactory) : ISeeder
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Seed()
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
