using CIS.Core.Entities.Barangays;
using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;
using NHibernate;
using System.Linq;

namespace CIS.UI.Features.Barangays.Maintenances.Purposes;

public class PurposeDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Execute()
    {
        var data = new string[] 
        {
            "Local Employment",
            "Travel Abroad"
        };

        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var purposes = session.QueryOver<Purpose>().Cacheable().List();
        var properCasing = session.QueryOver<ProperCasingConfiguration>().Cacheable().SingleOrDefault();

        if (!properCasing.IsProperCasingInitialized)
        {
            foreach (var purpose in purposes)
            {
                purpose.Name = purpose.Name.ToProperCase();
            }
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
