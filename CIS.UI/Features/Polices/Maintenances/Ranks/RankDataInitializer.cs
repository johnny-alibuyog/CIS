using System.Linq;
using CIS.Core.Entities.Polices;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Polices.Maintenances.Ranks;

public class RankDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var ranks = session.Query<Rank>().Cacheable().ToList();

        foreach (var toSave in Rank.All)
        {
            if (ranks.Contains(toSave))
                continue;

            session.Save(toSave);
        }

        transaction.Commit();
    }
}
