using System;
using System.Linq;
using CIS.Core.Domain.Common;
using NHibernate;
using NHibernate.Linq;

namespace CIS.Data.Definition.Common;

public class TerminalSeeder(ISessionFactory sessionFactory) : ISeeder
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public Action<Terminal> Updated { private get; set; }

    public void Seed()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session.Query<Terminal>()
            .Where(x => x.MachineName == Environment.MachineName)
            .ToFutureValue();

        var terminal = query.Value;
        if (terminal == null)
        {
            terminal = Terminal.GetLocalTerminal();
            session.Save(terminal);
        }

        this.Updated?.Invoke(terminal);

        transaction.Commit();
    }
}
