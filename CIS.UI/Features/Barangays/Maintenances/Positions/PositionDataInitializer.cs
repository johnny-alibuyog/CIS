using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Barangays.Maintenances.Positions
{
    public class PositionDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public PositionDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

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
}
