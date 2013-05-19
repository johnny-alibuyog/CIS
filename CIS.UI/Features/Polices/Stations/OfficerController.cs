using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Stations
{
    public class OfficerController : ControllerBase<OfficerViewModel>
    {
        public OfficerController(OfficerViewModel viewModel) : base(viewModel) 
        {
            PopulateLookup();

            this.ViewModel.Save = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value));
            this.ViewModel.Save.Subscribe(x => Save());
        }

        private void PopulateLookup()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transacction = session.BeginTransaction())
            {
                var ranks = session.Query<Rank>()
                    .Cacheable()
                    .ToFuture();

                this.ViewModel.Ranks = ranks
                    .Select(x => new Lookup<string>()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToReactiveColletion();

                transacction.Commit();
            }
        }

        public virtual void Populate(Guid id)
        {
            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Officer>()
                    .Where(x => x.Id == id)
                    .Fetch(x => x.Rank)
                    .ToFutureValue();

                var officer = query.Value;

                this.ViewModel.SerializeWith(officer);

                transaction.Commit();

                this.SessionProvider.ReleaseSharedSession();
            }
        }

        public virtual void Save()
        {
            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Station>()
                    .FetchMany(x => x.Officers)
                    .ThenFetch(x => x.Rank)
                    .ToFuture();

                var station = query.FirstOrDefault();
                if (station == null)
                {
                    station = new Station()
                    {
                        Name = "Name not set",
                        Location = "Location not set",
                        ClearanceValidity = "Clearance validity not set",
                    };
                }

                var officer = station.Officers.FirstOrDefault(x => x.Id == this.ViewModel.Id);
                if (officer == null)
                {
                    officer = new Officer();
                    station.AddOfficer(officer);
                }

                this.ViewModel.SerializeInto(officer);

                session.SaveOrUpdate(station);
                transaction.Commit();

                this.SessionProvider.ReleaseSharedSession();
            }

            this.ViewModel.ActionResult = true;
        }
    }
}
