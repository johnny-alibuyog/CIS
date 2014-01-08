using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Commons.Terminals
{
    public class TerminalDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public TerminalDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
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
    }
}
