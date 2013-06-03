using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using CIS.UI.Features.Commons.Signatures;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Maintenances
{
    public class OfficerController : ControllerBase<OfficerViewModel>
    {
        public OfficerController(OfficerViewModel viewModel) : base(viewModel) 
        {
            PopulateLookup();

            this.ViewModel.CaptureSignature = new ReactiveCommand();
            this.ViewModel.CaptureSignature.Subscribe(x => CaptureSignature());

            this.ViewModel.Load = new ReactiveCommand();
            this.ViewModel.Load.Subscribe(x => Load((Guid)x));


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

        public virtual void CaptureSignature()
        {
            var dialog = new DialogService<SignatureDialogView, SignatureDialogViewModel>();
            dialog.ViewModel.Signature.SignatureImage = this.ViewModel.Signature;

            var result = dialog.ShowModal(this, "Signature", null);
            if (result != null)
                this.ViewModel.Signature = result.Signature.SignatureImage;
        }

        public virtual void Load(Guid id)
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
                        Office = "Office not set",
                        Location = "Location not set",
                        ClearanceValidityInDays = 60,
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
