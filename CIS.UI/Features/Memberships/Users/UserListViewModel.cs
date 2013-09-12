using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;

namespace CIS.UI.Features.Memberships.Users
{
    public class UserListViewModel : ViewModelBase
    {
        private readonly UserListController _controller;

        public virtual UserListCriteriaViewModel Criteria { get; set; }

        public virtual UserListItemViewModel SelectedItem { get; set; }

        public virtual IReactiveList<UserListItemViewModel> Items { get; set; }

        public virtual IReactiveCommand Search { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public UserListViewModel()
        {
            //_controller = new UserListController(this);
            _controller = IoC.Container.Resolve<UserListController>(new ViewModelDependency(this));
        }
    }
}
