﻿using System;
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
            var result = dialog.ShowModal(this, "Create License", null);
            if (result != null)
            {
                var item = new LicenseListItemViewModel()
                {
                    Id = result.Id,
                    Owner = result.Person.FullName,
                    Gun = result.Gun.Kind.Name + ": " + result.Gun.Model,
                };

                this.ViewModel.Items.Add(item);
                this.ViewModel.SelectedItem = item;
            }
                this.Search();
        }

        public virtual void Edit(LicenseListItemViewModel item)
        {
            var dialog = new DialogService<LicenseView, LicenseViewModel>();
            dialog.ViewModel.Load.Execute(item.Id);
            var result = dialog.ShowModal(this, "Edit License", null);
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
