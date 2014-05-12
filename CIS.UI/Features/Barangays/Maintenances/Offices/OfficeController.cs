using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Context;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Barangays.Maintenances.Offices
{
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
            this.ViewModel.Refresh.Subscribe(x => Load(confirm: true));
            this.ViewModel.Refresh.ThrownExceptions.Handle(this);
        }

        public virtual void LookupLogo()
        {
            var openImageDialog = IoC.Container.Resolve<IOpenImageDialogService>();
            var logo = openImageDialog.Show();
            if (logo != null)
                this.ViewModel.Logo = logo;
        }

        public virtual void Load(bool confirm = false)
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

            if (confirm)
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

            this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Station"));
        }
    }
}
