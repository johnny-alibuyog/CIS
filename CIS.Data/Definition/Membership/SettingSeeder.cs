using System;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.Core.Domain.Membership;
using NHibernate;
using NHibernate.Linq;

namespace CIS.Data.Definition.Membership;

public class SettingSeeder(ISessionFactory sessionFactory) : ISeeder
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Seed()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session.Query<Setting>()
            .Where(x => x.Terminal.MachineName == Environment.MachineName)
            .Fetch(x => x.Terminal)
            .ToFutureValue();

        var entity = query.Value;
        if (entity == null)
        {
            entity = Setting.Default;
            entity.Terminal = session.Query<Terminal>().FirstOrDefault(x => x.MachineName == Environment.MachineName);
            session.Save(entity);
        }

        transaction.Commit();
    }
}
