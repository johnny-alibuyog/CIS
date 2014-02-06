using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Memberships;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;

namespace CIS.UI.Features.Memberships.Users.MasterList
{
    //[HandleError]
    public class UserListController : ControllerBase<UserListViewModel>
    {
        public UserListController(UserListViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Criteria = new UserListCriteriaViewModel();
            this.ViewModel.Items = new ReactiveList<UserListItemViewModel>();

            var isAdministrator = App.Data.User.IsPoliceAdministartor();

            this.ViewModel.Search = new ReactiveCommand();
            this.ViewModel.Search.Subscribe(x => Search());
            this.ViewModel.Search.ThrownExceptions.Handle(this);

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.CanExecute(isAdministrator);
            this.ViewModel.Create.Subscribe(x => Create());
            this.ViewModel.Create.ThrownExceptions.Handle(this);

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.CanExecute(isAdministrator);
            this.ViewModel.Edit.Subscribe(x => Edit((UserListItemViewModel)x));
            this.ViewModel.Edit.ThrownExceptions.Handle(this);

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.CanExecute(isAdministrator);
            this.ViewModel.Delete.Subscribe(x => Delete((UserListItemViewModel)x));
            this.ViewModel.Delete.ThrownExceptions.Handle(this);

            this.Search();
        }

        private UserViewModel New()
        {
            var viewModel = IoC.Container.Resolve<UserViewModel>();
            viewModel.Roles = Enum.GetValues(typeof(Role)).Cast<Role>()
                .Select(x => new UserRoleViewModel(false, x))
                .ToReactiveList();
            return viewModel;
        }

        private UserViewModel Get(Guid id)
        {
            var viewModel = IoC.Container.Resolve<UserViewModel>();
            viewModel.Roles = Enum.GetValues(typeof(Role)).Cast<Role>()
                .Select(x => new UserRoleViewModel(false, x))
                .ToReactiveList();

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var userQuery = session.Query<User>()
                    .Where(x => x.Id == id)
                    .Fetch(x => x.Roles)
                    .ToFutureValue();

                var user = userQuery.Value;

                viewModel.SerializeWith(user);

                transaction.Commit();
            }
            return viewModel;
        }

        public virtual void Search()
        {
            var criteria = this.ViewModel.Criteria;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<User>();

                if (!string.IsNullOrWhiteSpace(criteria.FirstName))
                    query = query.Where(x => x.Person.FirstName.StartsWith(criteria.FirstName));

                if (!string.IsNullOrWhiteSpace(criteria.MiddleName))
                    query = query.Where(x => x.Person.MiddleName.StartsWith(criteria.MiddleName));

                if (!string.IsNullOrWhiteSpace(criteria.LastName))
                    query = query.Where(x => x.Person.LastName.StartsWith(criteria.LastName));

                if (!string.IsNullOrWhiteSpace(criteria.Username))
                    query = query.Where(x => x.Username.StartsWith(criteria.Username));

                this.ViewModel.Items = query
                    .Select(x => new UserListItemViewModel()
                    {
                        Id = x.Id,
                        Username = x.Username,
                        Email = x.Email,
                        FullName = x.Person.FirstName + " " +
                            x.Person.MiddleName + " " + x.Person.LastName
                    })
                    .ToReactiveList();

                transaction.Commit();
            }
        }

        //[Authorize(Roles = new Role[] { Role.PoliceAdministartor })]
        public virtual void Create()
        {
            this.Authorize(new Role[] { Role.PoliceAdministartor });

            var dialog = new DialogService<UserView, UserViewModel>();
            dialog.ViewModel.SerializeWith(New());

            dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
            dialog.ViewModel.Save.Subscribe(x => Insert(dialog.ViewModel));
            dialog.ViewModel.Save.ThrownExceptions.Handle(this);

            dialog.ShowModal(this, "Create User");
        }

        //[Authorize(Roles = new Role[] { Role.PoliceAdministartor })]
        public virtual void Insert(UserViewModel value)
        {
            this.Authorize(new Role[] { Role.PoliceAdministartor });

            var message = string.Format("Are you sure you want to save user {0}?", value.Username);
            var confirmed = this.MessageBox.Confirm(message, "Save");
            if (confirmed == false)
                return;

            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var exists = session.Query<User>().Any(x => x.Username == value.Username);
                if (exists)
                {
                    this.MessageBox.Warn(string.Format("User {0} already exists.", value.Username));
                    return;
                }

                var user = new User();

                value.DeserializeInto(user);

                session.Save(user);

                transaction.Commit();

                this.SessionProvider.ReleaseSharedSession();
            }

            this.MessageBox.Inform("Save has been successfully completed.");

            this.Search();

            value.Close();
        }

        //[Authorize(Roles = new Role[] { Role.PoliceAdministartor })]
        public virtual void Edit(UserListItemViewModel item)
        {
            this.Authorize(new Role[] { Role.PoliceAdministartor });
            
            this.ViewModel.SelectedItem = item;

            var dialog = new DialogService<UserView, UserViewModel>();

            dialog.ViewModel.SerializeWith(Get(item.Id));
            dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
            dialog.ViewModel.Save.Subscribe(x => this.Update(dialog.ViewModel));
            dialog.ViewModel.Save.ThrownExceptions.Handle(this);

            dialog.ShowModal(this, "Edit User");
        }

        //[Authorize(Roles = new Role[] { Role.PoliceAdministartor })]
        public virtual void Update(UserViewModel value)
        {
            this.Authorize(new Role[] { Role.PoliceAdministartor });

            var message = string.Format("Are you sure you want to save user {0}?", value.Username);
            var confirmed = this.MessageBox.Confirm(message, "Save");
            if (confirmed == false)
                return;

            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var userQuery = session.Query<User>()
                    .Where(x => x.Id == value.Id)
                    .Fetch(x => x.Roles)
                    .ToFutureValue();

                var user = userQuery.Value;

                value.DeserializeInto(user);

                transaction.Commit();

                this.SessionProvider.ReleaseSharedSession();
            }

            this.MessageBox.Inform("Save has been successfully completed.");

            this.Search();

            value.Close();
        }

        //[Authorize(Roles = new Role[] { Role.PoliceAdministartor })]
        public virtual void Delete(UserListItemViewModel item)
        {
            this.Authorize(new Role[] { Role.PoliceAdministartor });

            this.ViewModel.SelectedItem = item;

            var message = string.Format("Are you sure you want to delete user {0}?", item.Username);
            var confirmed = this.MessageBox.Confirm(message, "Delete");
            if (confirmed == false)
                return;

            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var user = session.Get<User>(item.Id);

                session.Delete(user);

                transaction.Commit();

                this.SessionProvider.ReleaseSharedSession();
            }

            this.MessageBox.Inform("Delete has been successfully completed.");

            this.Search();
        }
    }
}
