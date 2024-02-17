using System.Collections.Generic;
using System.Linq;
using CIS.Core.Domain.Common;
using NHibernate;
using NHibernate.Linq;

namespace CIS.Data.Definition.Common;

public class CitizenshipSeeder(ISessionFactory sessionFactory) : ISeeder
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Seed()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        if (session.Query<Citizenship>().Any())
            return;

        Citizenship.List.ForEach(x => session.Save(x));

        transaction.Commit();
    }
}
