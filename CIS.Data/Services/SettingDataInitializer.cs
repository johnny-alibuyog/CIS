using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using NHibernate;
using NHibernate.Linq;

namespace CIS.Data.Services
{
    public class SettingDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public SettingDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
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
    }
}
