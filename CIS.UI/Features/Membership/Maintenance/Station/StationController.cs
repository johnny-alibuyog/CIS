﻿using System;
using System.Linq;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using NHibernate.Linq;
using ReactiveUI;
using MembershipD = CIS.Core.Domain.Membership;

namespace CIS.UI.Features.Membership.Maintenance.Station;

[HandleError]
public class StationController : ControllerBase<StationViewModel>
{
    public StationController(StationViewModel viewModel)
        : base(viewModel)
    {
        this.Load();

        this.ViewModel.LookupLogo = new ReactiveCommand();
        this.ViewModel.LookupLogo.Subscribe(x => LookupLogo());
        this.ViewModel.LookupLogo.ThrownExceptions.Handle(this);

        this.ViewModel.Save = new ReactiveCommand(this.ViewModel.IsValidObservable());
        this.ViewModel.Save.Subscribe(x => Save());
        this.ViewModel.Save.ThrownExceptions.Handle(this);

        this.ViewModel.Refresh = new ReactiveCommand();
        this.ViewModel.Refresh.Subscribe(x => Load(inform: true));
        this.ViewModel.Refresh.ThrownExceptions.Handle(this);
    }

    public virtual void LookupLogo()
    {
        var openImageDialog = IoC.Container.Resolve<IOpenImageDialogService>();
        var logo = openImageDialog.Show();
        if (logo != null)
            this.ViewModel.Logo = logo;
    }

    public virtual void Load(bool inform = false)
    {
        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var query = session.Query<MembershipD.Station>()
                .Fetch(x => x.Logo)
                .ToFuture();

            var station = query.FirstOrDefault();
            if (station != null)
                this.ViewModel.SerializeWith(station);

            transaction.Commit();
        }

        if (!inform)
            return;
        
        this.MessageBox.Inform("Station configuration has loaded.", "Station");
    }

    public virtual void Save()
    {
        var confirmed = this.MessageBox.Confirm("Do you want to save changes?.", "Save");
        if (confirmed == false)
            return;

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var query = session.Query<MembershipD.Station>()
                .Fetch(x => x.Logo)
                .ToFuture();

            var station = Enumerable.FirstOrDefault<Core.Domain.Membership.Station>(query) ?? new Core.Domain.Membership.Station();

            this.ViewModel.DeserializeInto(station);

            session.SaveOrUpdate(station);
            transaction.Commit();
        }

        this.MessageBox.Inform("Save has been successfully completed.");
        //this.MessageBox.Inform("Station configuration has been saved.", "Station");

        this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Station"));
    }
}