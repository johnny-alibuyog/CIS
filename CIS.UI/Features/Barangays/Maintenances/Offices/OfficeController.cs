using CIS.Core.Entities.Barangays;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using NHibernate.Linq;
using ReactiveUI;
using System;
using System.Linq;

namespace CIS.UI.Features.Barangays.Maintenances.Offices;

public class OfficeController: ControllerBase<OfficeViewModel>
{
    public OfficeController(OfficeViewModel viewModel)
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
        if (logo == null)
            return;
        
        this.ViewModel.Logo = logo;
    }

    public virtual void Load(bool inform = false)
    {
        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var query = session.Query<Office>()
                .Fetch(x => x.Logo)
                .ToFuture();

            var office = query.FirstOrDefault();
            if (office != null)
                this.ViewModel.SerializeWith(office);

            transaction.Commit();
        }

        if (!inform)
            return;
        
        this.MessageBox.Inform("Station configuration has been loaded.", "Station");
    }

    public virtual void Save()
    {
        var confirmed = this.MessageBox.Confirm("Do you want to save changes?.", "Save");
        if (confirmed == false)
            return;

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var query = session.Query<Office>()
                .Fetch(x => x.Logo)
                .ToFuture();

            var office = query.FirstOrDefault();
            if (office == null)
                office = new Office();

            this.ViewModel.DeserializeInto(office);

            session.SaveOrUpdate(office);
            transaction.Commit();
        }

        this.MessageBox.Inform("Save has been successfully completed.");
        //this.MessageBox.Inform("Station configuration has been saved.", "Station");

        this.MessageBus.SendMessage(new MaintenanceMessage("Station"));
    }
}
