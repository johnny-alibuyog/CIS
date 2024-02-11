using System;
using System.Linq;
using CIS.Core.Domain.Membership;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.Extentions;
using NHibernate.Linq;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Warrants.MasterList;

[HandleError]
public class WarrantController : ControllerBase<WarrantViewModel>
{
    public WarrantController(WarrantViewModel viewModel)
        : base(viewModel)
    {
        this.ViewModel.Load = new ReactiveCommand();
        this.ViewModel.Load.Subscribe(x => Load((Guid)x));
        this.ViewModel.Load.ThrownExceptions.Handle(this);

        this.ViewModel.CreateSupect = new ReactiveCommand();
        this.ViewModel.CreateSupect.Subscribe(x => CreateSuspect());
        this.ViewModel.CreateSupect.ThrownExceptions.Handle(this);

        this.ViewModel.EditSuspect = new ReactiveCommand();
        this.ViewModel.EditSuspect.Subscribe(x => EditSuspect((SuspectViewModel)x));
        this.ViewModel.EditSuspect.ThrownExceptions.Handle(this);

        this.ViewModel.DeleteSuspect = new ReactiveCommand();
        this.ViewModel.DeleteSuspect.Subscribe(x => DeleteSuspect((SuspectViewModel)x));
        this.ViewModel.DeleteSuspect.ThrownExceptions.Handle(this);

        this.ViewModel.BatchSave = new ReactiveCommand(this.ViewModel.IsValidObservable());
        this.ViewModel.BatchSave.Subscribe(x => BatchSave());
        this.ViewModel.BatchSave.ThrownExceptions.Handle(this);
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
        var dialog = new DialogService<SuspectViewModel>();
        var value = dialog.ShowModal(this, "Create Suspect");
        if (value != null)
            this.ViewModel.Suspects.Add(value);
    }

    public virtual void EditSuspect(SuspectViewModel item)
    {
        this.ViewModel.SelectedSuspect = item;

        var dialog = new DialogService<SuspectViewModel>();
        var value = dialog.ShowModal(this, "Edit Susptect", this.ViewModel.SelectedSuspect);
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
        var message = string.Format("Do you want to save warrant?");
        var confirmed = this.MessageBox.Confirm(message, "Save");
        if (confirmed == false)
            return;

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var warrant = default(Warrant);

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

            this.ViewModel.DeserializeInto(warrant);

            transaction.Commit();
        }

        this.MessageBox.Inform("Save has been successfully completed.");

        this.ViewModel.Close();
    }
}
