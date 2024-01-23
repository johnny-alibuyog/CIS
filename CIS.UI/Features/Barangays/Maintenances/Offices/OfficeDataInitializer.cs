using CIS.Core.Entities.Barangays;
using CIS.Core.Entities.Commons;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace CIS.UI.Features.Barangays.Maintenances.Offices;

public class OfficeDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var office = session.Query<Office>().FirstOrDefault();
        if (office == null)
        {
            office = new Office()
            {
                Name = "Name not set",
                Location = "Location not set",
                ClearanceFee = 500.00M,
                CertificationFee = 30.00M,
                DocumentStampTax = 15.00M,
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
