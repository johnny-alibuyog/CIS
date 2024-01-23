using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Firearms;
using CIS.Core.Utilities.Extentions;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace CIS.UI.Features.Firearms.Maintenances.Kinds;

public class KindDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public virtual IEnumerable<string> Data { get; set; } =
    [
        "Pistol",
        "Revolver",
        "Shotgun",
        "Rifle",
        "Carbine",
        "Assault Rifle",
        "Bullpup Rifle",
        "Battle Rifle",
        "Machine Pistol",
        "Submachine Gun",
        "Machine Gun",
        "Light Machine Gun",
        "Medium Machine Gun",
        "Heavy Machine Guns",
    ];

    public void Execute()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var kinds = session.QueryOver<Kind>().Cacheable().List();
        var properCasing = session.QueryOver<ProperCasingConfiguration>().Cacheable().SingleOrDefault();

        if (!properCasing.IsProperCasingInitialized)
        {
            foreach (var kind in kinds)
            {
                kind.Name = kind.Name.ToProperCase();
            }
        }

        foreach (var item in this.Data)
        {
            //if (kinds.Any(x => x.Name == item))
            if (kinds.Any(x => x.Name.IsEqualTo(item)))
                continue;

            var kind = new Kind();
            kind.Name = item;

            session.Save(kind);
        }

        transaction.Commit();
    }
}
