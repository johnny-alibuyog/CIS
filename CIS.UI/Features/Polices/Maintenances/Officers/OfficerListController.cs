using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Memberships;
using CIS.Core.Entities.Polices;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Features.Commons.Signatures;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Exceptions;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Maintenances.Officers
{
    [HandleError]
    public class OfficerListController : ControllerBase<OfficerListViewModel>
    {
        public OfficerListController(OfficerListViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Criteria = new OfficerListCriteriaViewModel();

            this.ViewModel.Search = new ReactiveCommand();
            this.ViewModel.Search.Subscribe(x => Search());
            this.ViewModel.Search.ThrownExceptions.Handle(this);

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());
            this.ViewModel.Create.ThrownExceptions.Handle(this);

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => Edit((OfficerListItemViewModel)x));
            this.ViewModel.Edit.ThrownExceptions.Handle(this);

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((OfficerListItemViewModel)x));
            this.ViewModel.Delete.ThrownExceptions.Handle(this);

            this.Search();
        }

        private void CaptureSignature(OfficerViewModel viewModel)
        {
            var dialog = new DialogService<SignatureDialogViewModel>();
            dialog.ViewModel.Signature.SignatureImage = viewModel.Signature;

            var result = dialog.ShowModal(this, "Signature", null);
            if (result != null)
                viewModel.Signature = result.Signature.SignatureImage;
        }

        private OfficerViewModel New()
        {
            var viewModel = IoC.Container.Resolve<OfficerViewModel>();
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var ranks = session.Query<Rank>().Cacheable().ToList();
                viewModel.Ranks = ranks.Select(x => new Lookup<string>(x.Id, x.Name)).ToReactiveList();

                transaction.Commit();
            }
            return viewModel;
        }

        private OfficerViewModel Get(Guid id)
        {
            var viewModel = IoC.Container.Resolve<OfficerViewModel>();
            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var rankQuery = session.Query<Rank>().Cacheable().ToList();

                var officerQuery = session.Query<Officer>()
                    .Where(x => x.Id == id)
                    .Fetch(x => x.Rank)
                    .ToFutureValue();

                viewModel.Ranks = rankQuery.Select(x => new Lookup<string>(x.Id, x.Name)).ToReactiveList();
                viewModel.SerializeWith(officerQuery.Value);

                transaction.Commit();

                this.SessionProvider.ReleaseSharedSession();
            }
            return viewModel;
        }

        public virtual void Search()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Officer>();

                if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.FirstName))
                    query = query.Where(x => x.Person.FirstName.StartsWith(this.ViewModel.Criteria.FirstName));

                if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.MiddleName))
                    query = query.Where(x => x.Person.MiddleName.StartsWith(this.ViewModel.Criteria.MiddleName));

                if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.LastName))
                    query = query.Where(x => x.Person.LastName.StartsWith(this.ViewModel.Criteria.LastName));

                this.ViewModel.Items = query
                    .OrderBy(x => x.Person.FirstName)
                    .ThenBy(x => x.Person.MiddleName)
                    .ThenBy(x => x.Person.LastName)
                    .Select(x => new OfficerListItemViewModel()
                    {
                        Id = x.Id,
                        Name = x.Person.FirstName + " " + x.Person.MiddleName + " " + x.Person.LastName,
                        Rank = x.Rank.Name,
                    })
                    .ToReactiveList();

                transaction.Commit();
            }
        }

        //[Authorize(Roles = new Role[] { Role.PoliceAdministartor, Role.PoliceEncoder })]
        public virtual void Create()
        {
            var dialog = new DialogService<OfficerViewModel>();
            dialog.ViewModel.SerializeWith(New());

            dialog.ViewModel.CaptureSignature = new ReactiveCommand();
            dialog.ViewModel.CaptureSignature.Subscribe(x => CaptureSignature(dialog.ViewModel));
            dialog.ViewModel.CaptureSignature.ThrownExceptions.Handle(this);

            dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
            dialog.ViewModel.Save.Subscribe(x => Insert(dialog.ViewModel));
            dialog.ViewModel.Save.ThrownExceptions.Handle(this);

            dialog.ShowModal(this, "Create Officer", null);
        }

        public virtual void Insert(OfficerViewModel value)
        {
            var message = string.Format("Do you want to save officer {0} {1}.", value.Rank.Name, value.Person.FullName);
            var confirmed = this.MessageBox.Confirm(message, "Save");
            if (confirmed == false)
                return;

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
                        ClearanceFee = 100.00M,
                        ClearanceValidityInDays = 60,
                    };
                }

                var officer = station.Officers.FirstOrDefault(x => x.Id == value.Id);
                if (officer == null)
                {
                    officer = new Officer();
                    station.AddOfficer(officer);
                }

                value.DeserializeInto(officer);

                session.SaveOrUpdate(station);
                transaction.Commit();

                value.Id = officer.Id;

                this.SessionProvider.ReleaseSharedSession();
            }

            this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Officer"));

            this.MessageBox.Inform("Save has been successfully completed.");

            //var item = new OfficerListItemViewModel();
            //item.Id = value.Id;
            //item.Name = value.Person.FullName;
            //item.Rank = value.Rank.Name;

            //this.ViewModel.Items.Add(item);
            //this.ViewModel.SelectedItem = item;

            this.Search();

            value.Close();
        }

        public virtual void Edit(OfficerListItemViewModel item)
        {
            this.ViewModel.SelectedItem = item;

            var dialog = new DialogService<OfficerViewModel>();
            dialog.ViewModel.SerializeWith(Get(item.Id));

            dialog.ViewModel.CaptureSignature = new ReactiveCommand();
            dialog.ViewModel.CaptureSignature.Subscribe(x => CaptureSignature(dialog.ViewModel));
            dialog.ViewModel.CaptureSignature.ThrownExceptions.Handle(this);

            dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
            dialog.ViewModel.Save.Subscribe(x => Update(dialog.ViewModel));
            dialog.ViewModel.Save.ThrownExceptions.Handle(this);

            dialog.ShowModal(this, "Edit Officer", null);
        }

        public virtual void Update(OfficerViewModel value)
        {
            var message = string.Format("Do you want to save officer {0} {1}.", value.Rank, value.Person.FullName);
            var confirmed = this.MessageBox.Confirm(message, "Save");
            if (confirmed == false)
                return;

            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Officer>()
                    .Where(x => x.Id == value.Id)
                    .Fetch(x => x.Rank)
                    .ToFutureValue();

                var officer = query.Value;
                value.DeserializeInto(query.Value);

                transaction.Commit();

                this.SessionProvider.ReleaseSharedSession();
            }

            this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Officer"));

            this.MessageBox.Inform("Save has been successfully completed.");

            //var item = this.ViewModel.SelectedItem;
            //item.Id = value.Id;
            //item.Name = value.Person.FullName;
            //item.Rank = value.Rank.Name;

            this.Search();

            value.Close();
        }

        public virtual void Delete(OfficerListItemViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to delete officer {0} {1}.", item.Rank, item.Name);
                var confirmed = this.MessageBox.Confirm(message, "Delete");
                if (confirmed == false)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var officer = session.Load<Officer>(item.Id);

                    session.Delete(officer);
                    transaction.Commit();
                }

                this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Officer"));

                this.MessageBox.Inform("Delete has been successfully completed.");

                this.Search();

                //this.ViewModel.Items.Remove(item);
                //this.ViewModel.SelectedItem = null;
            }
            catch (GenericADOException)
            {
                throw new InvalidOperationException(string.Format("Unable to delete. Officer {0} may already be in use.", item.Name));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
