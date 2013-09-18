using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Memberships;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;

namespace CIS.UI.Features.Memberships.Users
{
    public class UserListController : ControllerBase<UserListViewModel>
    {
        public UserListController(UserListViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.Criteria = new UserListCriteriaViewModel();
            this.ViewModel.Items = new ReactiveList<UserListItemViewModel>();

            this.ViewModel.Search = new ReactiveCommand();
            this.ViewModel.Search.Subscribe(x => this.Search());

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => this.Create());

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => this.Edit((Guid)x));

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => this.Delete((Guid)x));
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

                viewModel.SerializeWith(userQuery.Value);

                transaction.Commit();
            }
            return viewModel;
        }


        [HandleError]
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

        [HandleError]
        public virtual void Create()
        {
            var dialog = new DialogService<UserView, UserViewModel>();
            dialog.ViewModel = New();
            dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
            dialog.ViewModel.Save.Subscribe(x => Insert(dialog.ViewModel));
            dialog.ShowModal(this, "Create User");
        }

        [HandleError]
        public virtual void Insert(UserViewModel value)
        {
            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var user = new User();

                value.SerializeInto(user);

                transaction.Commit();
            }

            this.Search();
        }

        [HandleError]
        public virtual void Edit(Guid id)
        {
            var dialog = new DialogService<UserView, UserViewModel>();
            dialog.ViewModel = Get(id);
            dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
            dialog.ViewModel.Save.Subscribe(x => Insert(dialog.ViewModel));
            dialog.ShowModal(this, "Edit User");
        }

        [HandleError]
        public virtual void Update(UserViewModel value)
        {
            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var userQuery = session.Query<User>()
                    .Where(x => x.Id == value.Id)
                    .Fetch(x => x.Roles)
                    .ToFutureValue();

                var user = userQuery.Value;

                value.SerializeInto(user);

                transaction.Commit();
            }

            this.Search();
        }

        [HandleError]
        public virtual void Delete(Guid id)
        {
            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var user = session.Get<User>(id);
                
                session.Delete(user);

                transaction.Commit();
            }

            this.Search();
        }
    }
}
