using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Memberships;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;

namespace CIS.UI.Features.Memberships.Users
{
    public class UserListController : ControllerBase<UserListViewModel>
    {
        private IReactiveList<UserRoleViewModel> _roles;

        public UserListController(UserListViewModel viewModel) : base(viewModel)
        {
            this.PopulatePulldown();

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

        private void PopulatePulldown()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var roles = session.Query<Role>().Cacheable().ToList();

                _roles = roles
                    .Select(x => new UserRoleViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Checked = false
                    })
                    .ToReactiveList();

                transaction.Commit();
            }
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

        public virtual void Create()
        {

        }

        public virtual void Insert(UserViewModel value)
        {
        }

        public virtual void Edit(Guid id)
        {
        }

        public virtual void Update(UserViewModel value) 
        { 
        }

        public virtual void Delete(Guid id)
        {
        }
    }
}
