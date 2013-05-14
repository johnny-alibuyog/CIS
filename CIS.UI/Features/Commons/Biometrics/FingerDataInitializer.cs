using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Commons.Biometrics
{
    public class FingerDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public FingerDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;

        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var fingers = session.Query<Finger>()
                    .Cacheable()
                    .ToFuture();

                foreach (var item in Finger.All)
                {
                    if (fingers.Contains(item))
                        continue;

                    session.Save(item);
                }

                transaction.Commit();
            }
        }
    }
}
