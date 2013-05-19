using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.UI.Utilities.CommonDialogs;
using FirstFloor.ModernUI.Windows.Controls;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Warrants
{
    public class WarrantListController : ControllerBase<WarrantListViewModel>
    {
        public WarrantListController(WarrantListViewModel viewModel) : base(viewModel)
        {

            ViewModel.Search = new ReactiveCommand(
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
            ViewModel.Search.Subscribe(x => Search());

            ViewModel.Create = new ReactiveCommand();
            ViewModel.Create.Subscribe(x => Create());

            ViewModel.Edit = new ReactiveCommand();
            ViewModel.Edit.Subscribe(x => { Edit((WarrantListItemViewModel)x); });

            ViewModel.Delete = new ReactiveCommand();
            ViewModel.Delete.Subscribe(x => { Delete((WarrantListItemViewModel)x); });
        }

        public virtual void Search()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Suspect>();

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
                    .Select(x => new WarrantListItemViewModel()
                    {
                        Id = x.Warrant.Id,
                        SuspectId = x.Id,
                        Suspect = x.Person.FirstName + " " + 
                            x.Person.MiddleName + " " + x.Person.LastName,
                        Crime = x.Warrant.Crime,
                        IssuedOn = x.Warrant.IssuedOn
                    })
                    .ToList();

                this.ViewModel.Items = new ReactiveCollection<WarrantListItemViewModel>(items);

                transaction.Commit();
            }
        }

        public virtual void Create()
        {
            var dialog = new DialogService<WarrantView, WarrantViewModel>();
            var result = dialog.Show(this, "Create Warrant", null);
            if (result != null)
                this.Search();
        }

        public virtual void Edit(WarrantListItemViewModel item)
        {
            var dialog = new DialogService<WarrantView, WarrantViewModel>();
            dialog.ViewModel.Load.Execute(item.Id);
            var result = dialog.Show(this, "Edit Warrant", null);
            if (result != null)
                this.Search();
        }

        public virtual void Delete(WarrantListItemViewModel item)
        {
            this.ViewModel.SelectedItem = item;
            var selected = this.ViewModel.SelectedItem;
            if (selected == null)
                return;

            var message = string.Format("Are you sure you want to delete warrant for {0} with case {1}", selected.Suspect, selected.Crime);
            var confirm = MessageDialog.Show(message, "Delete", MessageBoxButton.YesNo);
            if (confirm == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Warrant>()
                    .Where(x => x.Id == selected.Id)
                    .FetchMany(x => x.Suspects)
                    .ToFutureValue();

                var warrant = query.Value;
                var suspect = warrant.Suspects.FirstOrDefault(x => x.Id == selected.SuspectId);
                if (suspect != null)
                {
                    warrant.DeleteSuspect(suspect);
                    session.Delete(suspect);
                }

                if (warrant.Suspects.Count() == 0)
                    session.Delete(warrant);

                transaction.Commit();
            }

            this.Search();
        }
    }
}
