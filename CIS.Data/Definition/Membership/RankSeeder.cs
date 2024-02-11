using System.Linq;
using CIS.Core.Domain.Membership;
using NHibernate;
using NHibernate.Linq;

namespace CIS.Data.Definition.Membership;

public class RankSeeder(ISessionFactory sessionFactory) : ISeeder
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Seed()
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
