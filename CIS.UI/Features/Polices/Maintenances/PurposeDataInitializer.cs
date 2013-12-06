using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using CIS.Core.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Polices.Maintenances
{
    public class PurposeDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public PurposeDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;

        }

        public void Execute()
        {
            var data = new string[] 
            {
                "Local Employment",
                "Travel Abroad"
            };

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var purposes = session.Query<Purpose>().Cacheable().ToList();

                foreach (var purpose in purposes)
                {
                    purpose.Name = purpose.Name.ToProperCase();
                }

                foreach (var item in data)
                {
                    if (purposes.Any(x => x.Name == item))
                        continue;

                    var purpose = new Purpose();
                    purpose.Name = item;

                    session.Save(purpose);
                }

                transaction.Commit();
            }
        }
    }
}
