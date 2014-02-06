using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Polices.Maintenances.Ranks
{
    public class RankDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public RankDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
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
    }
}
