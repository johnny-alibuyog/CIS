using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Warrants
{
    public class WarrantController : ControllerBase<WarrantViewModel>
    {
        public WarrantController(WarrantViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Load = new ReactiveCommand();
            this.ViewModel.Load.Subscribe(x => Load((Guid)x));

            this.ViewModel.CreateSupect = new ReactiveCommand();
            this.ViewModel.CreateSupect.Subscribe(x => CreateSuspect());

            this.ViewModel.EditSuspect = new ReactiveCommand();
            this.ViewModel.EditSuspect.Subscribe(x => EditSuspect((SuspectViewModel)x));

            this.ViewModel.DeleteSuspect = new ReactiveCommand();
            this.ViewModel.DeleteSuspect.Subscribe(x => DeleteSuspect((SuspectViewModel)x));

            this.ViewModel.BatchSave = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value));
            this.ViewModel.BatchSave.Subscribe(x => BatchSave());
        }

        public virtual void Load(Guid id)
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Warrant>()
                    .Where(x => x.Id == id)
                    .FetchMany(x => x.Suspects)
                    .ThenFetchMany(x => x.Aliases)
                    .ToFutureValue();

                session.Query<Warrant>()
                    .Where(x => x.Id == id)
                    .FetchMany(x => x.Suspects)
                    .ThenFetchMany(x => x.Occupations)
                    .ToFutureValue();

                var warrant = query.Value;
                
                this.ViewModel.SerializeWith(warrant);

                transaction.Commit();
            }
        }

        public virtual void CreateSuspect()
        {
            var dialog = new DialogService<SuspectView, SuspectViewModel>();
            var value = dialog.Show(this, "Create Susptect");
            if (value != null)
                this.ViewModel.Suspects.Add(value);
        }

        public virtual void EditSuspect(SuspectViewModel item)
        {
            this.ViewModel.SelectedSuspect = item;

            var dialog = new DialogService<SuspectView, SuspectViewModel>();
            var value = dialog.Show(this, "Edit Susptect", this.ViewModel.SelectedSuspect);
            if (value != null)
                this.ViewModel.SelectedSuspect.SerializeWith(value);
        }

        public virtual void DeleteSuspect(SuspectViewModel item)
        {
            this.ViewModel.Suspects.Remove(item);
            this.ViewModel.SelectedSuspect = null;
        }

        public virtual void BatchSave()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var warrant = (Warrant)null;

                if (this.ViewModel.Id == Guid.Empty)
                {
                    warrant = new Warrant();
                    session.Save(warrant);
                }
                else
                {
                    var query = session.Query<Warrant>()
                        .Where(x => x.Id == this.ViewModel.Id)
                        .FetchMany(x => x.Suspects)
                        .ThenFetchMany(x => x.Aliases)
                        .ToFutureValue();

                    session.Query<Warrant>()
                        .Where(x => x.Id == this.ViewModel.Id)
                        .FetchMany(x => x.Suspects)
                        .ThenFetchMany(x => x.Occupations)
                        .ToFutureValue();

                    warrant = query.Value;
                }

                this.ViewModel.SerializeInto(warrant);

                transaction.Commit();
            }

            this.ViewModel.ActionResult = true;
        }
    }
}
