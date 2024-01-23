using CIS.Core.Entities.Commons;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace CIS.UI.Features.Commons.Terminals;

public class TerminalDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
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

        App.Data.Terminal = terminal;

        transaction.Commit();
    }
}
