using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Xaml;
using NHibernate;
using NHibernate.Linq;
using CIS.Core.Entities.Polices;
using ReactiveUI;
using CIS.UI.Utilities.CommonDialogs;
using System.Windows;

namespace CIS.UI.Features.Polices.Stations
{
    public class OfficerListController : ControllerBase<OfficerListViewModel>
    {
        public OfficerListController(OfficerListViewModel viewModel)
            : base(viewModel)
        {
            ViewModel.Search = new ReactiveCommand();
            ViewModel.Search.Subscribe(x => Search());

            ViewModel.Create = new ReactiveCommand();
            ViewModel.Create.Subscribe(x => Create());

            ViewModel.Edit = new ReactiveCommand();
            ViewModel.Edit.Subscribe(x => { Edit((OfficerListItemViewModel)x); });

            ViewModel.Delete = new ReactiveCommand();
            ViewModel.Delete.Subscribe(x => { Delete((OfficerListItemViewModel)x); });
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

                var items = query
                    .OrderBy(x => x.Person.FirstName)
                    .ThenBy(x => x.Person.MiddleName)
                    .ThenBy(x => x.Person.LastName)
                    .Select(x => new OfficerListItemViewModel()
                    {
                        Id = x.Id,
                        Name = x.Person.FirstName + " " + x.Person.MiddleName + " " + x.Person.LastName,
                        Rank = x.Rank.Name,
                    })
                    .ToList();

                this.ViewModel.Items = new ReactiveCollection<OfficerListItemViewModel>(items);

                transaction.Commit();
            }
        }

        public virtual void Create()
        {
            var dialog = new DialogService<OfficerView, OfficerViewModel>();
            var result = dialog.Show(this, "Create Officer", null);
            if (result != null)
                this.Search();
        }

        public virtual void Edit(OfficerListItemViewModel item)
        {
            var dialog = new DialogService<OfficerView, OfficerViewModel>();
            dialog.ViewModel.Load.Execute(item.Id);
            var result = dialog.Show(this, "Edit Officer", null);
            if (result != null)
                this.Search();
        }

        public virtual void Delete(OfficerListItemViewModel item)
        {
            var message = string.Format("Are you sure you want to delete officer {0} {1}.", item.Rank, item.Name);
            var confirm = MessageDialog.Show(message, "Delete", MessageBoxButton.YesNo);
            if (confirm == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var officer = session.Load<Officer>(item.Id);

                session.Delete(officer);
                transaction.Commit();

            }
        }
    }
}
