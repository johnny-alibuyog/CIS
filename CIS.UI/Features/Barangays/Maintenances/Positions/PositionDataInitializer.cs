using CIS.Core.Entities.Barangays;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace CIS.UI.Features.Barangays.Maintenances.Positions;

public class PositionDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
    {
        using (var session = _sessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var positions = session.Query<Position>().Cacheable().ToList();

            foreach (var toSave in Position.All)
            {
                if (positions.Contains(toSave))
                    continue;

                session.Save(toSave);
            }

            transaction.Commit();
        }

        // load to cache
        using (var session = _sessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            session.Query<Position>().Cacheable().ToList();
            session.Query<Committee>().Cacheable().ToList();

            transaction.Commit();
        }
    }
}
