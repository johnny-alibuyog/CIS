using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using FirstFloor.ModernUI.Windows.Controls;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Warrants.MasterList
{
    [HandleError]
    public class WarrantListController : ControllerBase<WarrantListViewModel>
    {
        public WarrantListController(WarrantListViewModel viewModel)
            : base(viewModel)
        {

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
            this.ViewModel.Search.ThrownExceptions.Handle(this);

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());
            this.ViewModel.Create.ThrownExceptions.Handle(this);

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => Edit((WarrantListItemViewModel)x));
            this.ViewModel.Edit.ThrownExceptions.Handle(this);

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((WarrantListItemViewModel)x));
            this.ViewModel.Delete.ThrownExceptions.Handle(this);
        }

        public virtual void Search()
        {
            if (string.IsNullOrWhiteSpace(this.ViewModel.Criteria.FirstName) &&
                string.IsNullOrWhiteSpace(this.ViewModel.Criteria.MiddleName) &&
                string.IsNullOrWhiteSpace(this.ViewModel.Criteria.LastName))
                return;

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

                this.ViewModel.Items = new ReactiveList<WarrantListItemViewModel>(items);

                transaction.Commit();
            }
        }

        public virtual void Create()
        {
            var dialog = new DialogService<WarrantView, WarrantViewModel>();
            var result = dialog.ShowModal(this, "Create Warrant", null);
            if (result != null)
                this.Search();
        }

        public virtual void Edit(WarrantListItemViewModel item)
        {
            var dialog = new DialogService<WarrantView, WarrantViewModel>();
            dialog.ViewModel.Load.Execute(item.Id);
            var result = dialog.ShowModal(this, "Edit Warrant", null);
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
            var confirmed = this.MessageBox.Confirm(message, "Delete");
            if (confirmed == false)
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
