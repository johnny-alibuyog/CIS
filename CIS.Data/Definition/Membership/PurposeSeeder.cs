using System.Linq;
using CIS.Core.Domain.Common;
using CIS.Core.Utility.Extention;
using NHibernate;
using NHibernate.Linq;

namespace CIS.Data.Definition.Membership;

public class PurposeSeeder(ISessionFactory sessionFactory) : ISeeder
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public void Seed()
    {
        var data = new string[]
        {
            "Local Employment",
            "Travel Abroad"
        };

        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var purposes = session.QueryOver<Core.Domain.Membership.Purpose>().Cacheable().List();
        var properCasing = session.QueryOver<ProperCasingConfiguration>().Cacheable().SingleOrDefault();

        if (!properCasing.IsProperCasingInitialized)
            purposes.ForEach(x => x.Name = x.Name.ToProperCase());

        foreach (var item in data)
        {
            if (purposes.Any(x => x.Name == item))
                continue;

            var purpose = new Core.Domain.Membership.Purpose();
            purpose.Name = item;

            session.Save(purpose);
        }

        transaction.Commit();
    }
}
