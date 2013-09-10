using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Firearms;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class LicenseListController : ControllerBase<LicenseListViewModel>
    {
        public LicenseListController(LicenseListViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.Criteria = new LicenseListCriteriaViewModel();

            this.ViewModel.Search = new ReactiveCommand(
                this.ViewModel.WhenAny(
                    x => x.Criteria.FirstName,
                    x => x.Criteria.MiddleName,
                    x => x.Criteria.LastName,
                    (firstName, middleName, lastName) =>
                        !string.IsNullOrWhiteSpace(firstName.Value) ||
                        !string.IsNullOrWhiteSpace(middleName.Value) ||
                        !string.IsNullOrWhiteSpace(lastName.Value)
                )
            );
            this.ViewModel.Search.Subscribe(x => Search());

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => { Edit((LicenseListItemViewModel)x); });

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => { Delete((LicenseListItemViewModel)x); });
        }

        private LicenseViewModel New()
        {
            var viewModel = new LicenseViewModel();
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var makeQuery = session.Query<Make>().Cacheable().ToFuture();
                var kindQuery = session.Query<Kind>().Cacheable().ToFuture();

                viewModel.Gun.Makes = makeQuery.Select(x => new Lookup<Guid>(x.Id, x.Name)).ToReactiveList();
                viewModel.Gun.Kinds = kindQuery.Select(x => new Lookup<Guid>(x.Id, x.Name)).ToReactiveList();

                transaction.Commit();
            }
            return viewModel;
        }

        private LicenseViewModel Get(Guid id)
        {
            var viewModel = new LicenseViewModel();
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var makeQuery = session.Query<Make>().Cacheable().ToFuture();
                var kindQuery = session.Query<Kind>().Cacheable().ToFuture();
                var licenseQuery = session.Query<License>().Where(x => x.Id == id).ToFutureValue();

                viewModel.Gun.Makes = makeQuery.Select(x => new Lookup<Guid>(x.Id, x.Name)).ToReactiveList();
                viewModel.Gun.Kinds = kindQuery.Select(x => new Lookup<Guid>(x.Id, x.Name)).ToReactiveList();
                viewModel.SerializeWith(licenseQuery.Value);

                transaction.Commit();
            }
            return viewModel;
        }

        public virtual void Search()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<License>();

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
                    .Select(x => new LicenseListItemViewModel()
                    {
                        Id = x.Id,
                        Owner = x.Person.FirstName + " " +
                            x.Person.MiddleName + " " + x.Person.LastName,
                        Gun = x.Gun.Kind.Name + ": " + x.Gun.Model,
                        ExpiryDate = x.ExpiryDate
                    })
                    .ToReactiveList();

                transaction.Commit();
            }
        }

        public virtual void Create()
        {
            var dialog = new DialogService<LicenseView, LicenseViewModel>();
            dialog.ViewModel = New();
            dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
            dialog.ViewModel.Save.Subscribe(x => Insert((LicenseViewModel)x));
            dialog.ShowModal(this, "Create License", null);
        }

        public virtual void Insert(LicenseViewModel value)
        {
            var message = string.Format("Are you sure you want to save license?");
            var confirm = this.MessageBox.Confirm(message, "Save");
            if (confirm == false)
                return;

            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var license = new License();

                value.SerializeInto(license);

                session.Save(license);
                transaction.Commit();

                value.Id = license.Id;

                this.SessionProvider.ReleaseSharedSession();
            }

            this.MessageBox.Inform("Save has been successfully completed.");

            this.Search();

            value.Close();
        }

        public virtual void Edit(LicenseListItemViewModel item)
        {
            this.ViewModel.SelectedItem = item;

            var dialog = new DialogService<LicenseView, LicenseViewModel>();
            dialog.ViewModel = Get(item.Id);
            dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
            dialog.ViewModel.Save.Subscribe(x => Update((LicenseViewModel)x));
            dialog.ShowModal(this, "Edit License", null);
        }

        public virtual void Update(LicenseViewModel value)
        {
            var message = string.Format("Are you sure you want to save license?");
            var confirm = this.MessageBox.Confirm(message, "Save");
            if (confirm == false)
                return;

            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var license = session.Get<License>(value.Id);

                value.SerializeInto(license);

                session.Save(license);
                transaction.Commit();

                value.Id = license.Id;

                this.SessionProvider.ReleaseSharedSession();
            }

            this.MessageBox.Inform("Save has been successfully completed.");

            this.Search();

            //var item = this.ViewModel.SelectedItem;
            //item.Id = value.Id;
            //item.Owner = value.Person.FullName;
            //item.Gun = value.Gun.Kind.Name + ": " + value.Gun.Model;

            value.Close();
        }

        public virtual void Delete(LicenseListItemViewModel item)
        {
            this.ViewModel.SelectedItem = item;
            if (this.ViewModel.SelectedItem == null)
                return;

            var message = string.Format("Are you sure you want to delete license for {0} for gun {1}", item.Owner, item.Gun);
            var confirm = this.MessageBox.Confirm(message, "Delete");
            if (confirm == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<License>()
                    .Where(x => x.Id == item.Id)
                    .ToFutureValue();

                var license = query.Value;

                session.Delete(license);

                transaction.Commit();
            }

            this.MessageBox.Inform("Delete has been successfully completed.");

            this.Search();

            //this.ViewModel.Items.Remove(item);
            //this.ViewModel.SelectedItem = null;

            //this.Search();
        }
    }
}
