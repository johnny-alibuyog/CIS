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
        public LicenseListController(LicenseListViewModel viewModel)
            : base(viewModel)
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
            ViewModel.Edit.Subscribe(x => { Edit((LicenseListItemViewModel)x); });

            ViewModel.Delete = new ReactiveCommand();
            ViewModel.Delete.Subscribe(x => { Delete((LicenseListItemViewModel)x); });
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

                var items = query
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
                    .ToReactiveColletion();

                this.ViewModel.Items = items.ToReactiveColletion();

                transaction.Commit();
            }
        }

        public virtual void Create()
        {
            var dialog = new DialogService<LicenseView, LicenseViewModel>();
            var result = dialog.Show(this, "Create License", null);
            if (result != null)
                this.Search();
        }

        public virtual void Edit(LicenseListItemViewModel item)
        {
            var dialog = new DialogService<LicenseView, LicenseViewModel>();
            dialog.ViewModel.Load.Execute(item.Id);
            var result = dialog.Show(this, "Edit License", null);
            if (result != null)
                this.Search();
        }

        public virtual void Delete(LicenseListItemViewModel item)
        {
            this.ViewModel.SelectedItem = item;
            var selected = this.ViewModel.SelectedItem;
            if (selected == null)
                return;

            var message = string.Format("Are you sure you want to delete license for {0} for gun {1}", selected.Owner, selected.Gun);
            var confirm = MessageDialog.Show(message, "Delete", MessageBoxButton.YesNo);
            if (confirm == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<License>()
                    .Where(x => x.Id == selected.Id)
                    .ToFutureValue();

                var license = query.Value;

                session.Delete(license);

                transaction.Commit();
            }

            this.Search();
        }
    }
}
