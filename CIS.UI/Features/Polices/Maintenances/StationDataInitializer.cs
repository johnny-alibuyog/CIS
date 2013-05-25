using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Polices.Maintenances
{
    public class StationDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public StationDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var station = session.Query<Station>().FirstOrDefault();
                if (station == null)
                {
                    station = new Station()
                    {
                        Name = "Name not set",
                        Office = "Office not set",
                        Location = "Location not set",
                        ClearanceValidityInDays = 60,
                        Address = new Address()
                        {
                            Address1 = "addres1",
                            Address2 = "address2",
                            Barangay = "barangay",
                            City = "city",
                            Province = "province",
                        }
                    };

                    session.Save(station);
                }

                transaction.Commit();
            }
        }
    }
}
