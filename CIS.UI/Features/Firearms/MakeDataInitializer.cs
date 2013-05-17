﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Firearms;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Firearms
{
    public class MakeDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public MakeDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            var data = new string[] 
            {
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
            };

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var makes = session.Query<Make>()
                    .Cacheable()
                    .ToFuture();

                foreach (var item in data)
                {
                    if (makes.Any(x => x.Name == item))
                        continue;

                    var make = new Make();
                    make.Name = item;

                    session.Save(make);
                }

                transaction.Commit();
            }
        }
    }
}
