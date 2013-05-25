using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Firearms;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Firearms.Maintenances
{
    public class KindDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public virtual IEnumerable<string> Data { get; set; }

        public KindDataInitializer(ISessionFactory sessionFactory)
        {
            this.Data = new string[]
            {
                "Pistol",
                "Revolver",
                "Shotgun",
                "Rifle",
                "Carbine",
                "Assault Rifle",
                "Bullpup Rifle",
                "Battle Rifle",
                "Machine Pistol", 
                "Submachine Gun ",
                "Machine Gun",
                "Light Machine Gun",
                "Medium Machine Gun",
                "Heavy Machine Guns ",
            };

            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var kinds = session.Query<Kind>()
                    .Cacheable()
                    .ToFuture();

                foreach (var item in this.Data)
                {
                    if (kinds.Any(x => x.Name == item))
                        continue;

                    var kind = new Kind();
                    kind.Name = item;

                    session.Save(kind);
                }

                transaction.Commit();
            }
        }
    }
}
