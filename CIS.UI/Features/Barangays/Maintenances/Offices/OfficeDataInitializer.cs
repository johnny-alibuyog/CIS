using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using CIS.Core.Entities.Commons;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Barangays.Maintenances.Offices
{
    public class OfficeDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public OfficeDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var office = session.Query<Office>().FirstOrDefault();
                if (office == null)
                {
                    office = new Office()
                    {
                        Name = "Name not set",
                        Location = "Location not set",
                        ClearanceFee = 100.00M,
                        Address = new Address()
                        {
                            Address1 = "addres1",
                            Address2 = "address2",
                            Barangay = "barangay",
                            City = "city",
                            Province = "province",
                        }
                    };

                    session.Save(office);
                }

                transaction.Commit();
            }
        }
    }
}
