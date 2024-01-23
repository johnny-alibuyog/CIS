using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Firearms;
using CIS.Core.Utilities.Extentions;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace CIS.UI.Features.Firearms.Maintenances.Makes;

public class MakeDataInitializer(ISessionFactory sessionFactory) : IDataInitializer
{
    private readonly ISessionFactory _sessionFactory = sessionFactory;

    public virtual IEnumerable<string> Data { get; set; } =
    [
        "Aimpoint Red Dot Sights",
        "ArmaLite",
        "Armscor",
        "Auto-Ordnance",
        "Barrett",
        "Benelli",
        "Beretta",
        "Bersa",
        "Blaser",
        "Browning",
        "Bushmaster",
        "Century Arms",
        "CMMG",
        "Colt",
        "Crimson Trace Laser Sight Grips",
        "CZ Firearms",
        "Dan Wesson",
        "DPMS Rifles",
        "FNH",
        "Galco Holsters",
        "Galil",
        "Glock",
        "H&R Firearms",
        "Heckler and Koch",
        "Henry Firearms",
        "Hi-Point Firearms",
        "Hogue Grips",
        "Iver Johnson Firearms",
        "Kahr",
        "Kel Tec",
        "Kimber",
        "Knights Armament",
        "Magpul",
        "Makarov",
        "Marlin",
        "MasterPiece Arms",
        "Mauser",
        "Mossberg",
        "New England Firearms",
        "Norinco",
        "Perazzi",
        "Phoenix Arms",
        "Puma Firearms",
        "Remington",
        "Rock Island Armory",
        "Rock River Arms",
        "Rossi",
        "Ruger",
        "Saiga",
        "Sako",
        "Savage Arms",
        "Sig Sauer",
        "SKS",
        "Smith and Wesson",
        "Springfield Armory",
        "Stag Rifles",
        "Stevens Firearms",
        "Steyr Rifle",
        "Taurus Firearms",
        "Thompson/Center Firearms",
        "Uzi",
        "Walther",
        "Weatherby",
        "Wilson Combat",
        "Winchester",
     ];

    public void Execute()
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var makes = session.QueryOver<Make>().Cacheable().List();
        var properCasing = session.QueryOver<ProperCasingConfiguration>().Cacheable().SingleOrDefault();

        if (!properCasing.IsProperCasingInitialized)
        {
            foreach (var make in makes)
            {
                make.Name = make.Name.ToProperCase();
            }
        }

        foreach (var item in this.Data)
        {
            //if (makes.Any(x => x.Name == item))
            if (makes.Any(x => x.Name.IsEqualTo(item)))
                continue;

            var make = new Make();
            make.Name = item;

            session.Save(make);
        }

        transaction.Commit();
    }
}
