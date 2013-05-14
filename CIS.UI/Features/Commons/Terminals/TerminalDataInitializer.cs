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
                    .Where(x => x.PcName == "extreme")
                    .ToFutureValue();

                var terminal = query.Value;
                if (terminal == null)
                {
                    terminal = new Terminal()
                    {
                        PcName = "extreme",
                        IpAddress = "127.0.0.1",
                        MacAddress = "127.0.0.1",
                        WithDefaultLogin = false,
                        WithFingerPrintDevice = true,
                        FingersToScan = new List<Finger>()
                        {
                            Finger.RightThumb,
                            Finger.LeftThumb
                        }
                    };

                    session.Save(terminal);
                }

                transaction.Commit();
            }
        }
    }
}
