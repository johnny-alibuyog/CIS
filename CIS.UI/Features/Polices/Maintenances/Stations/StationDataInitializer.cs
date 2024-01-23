using System.Linq;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Polices.Maintenances.Stations;

public class StationDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var station = session.Query<Station>().FirstOrDefault();
        if (station == null)
        {
            station = new Station()
            {
                Name = "Name not set",
                Office = "Office not set",
                Location = "Location not set",
                ClearanceFee = 100.00M,
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
