using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Polices;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Exceptions;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Maintenances
{
    public class OfficerListController : ControllerBase<OfficerListViewModel>
    {
        public OfficerListController(OfficerListViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Criteria = new OfficerListCriteriaViewModel();

            this.ViewModel.Search = new ReactiveCommand();
            this.ViewModel.Search.Subscribe(x => Search());

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => { Edit((OfficerListItemViewModel)x); });

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => { Delete((OfficerListItemViewModel)x); });
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

                this.ViewModel.Items = query
                    .OrderBy(x => x.Person.FirstName)
                    .ThenBy(x => x.Person.MiddleName)
                    .ThenBy(x => x.Person.LastName)
                    .Select(x => new OfficerListItemViewModel()
                    {
                        Id = x.Id,
                        Name = x.Person.FirstName + " " + x.Person.MiddleName + " " + x.Person.LastName,
                        Rank = x.Rank.Name,
                    })
                    .ToReactiveList();

                transaction.Commit();
            }
        }

        public virtual void Create()
        {
            var dialog = new DialogService<OfficerView, OfficerViewModel>();
            var result = dialog.ShowModal(this, "Create Officer", null);
            if (result != null)
            {
                this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Officer"));

                var item = new OfficerListItemViewModel();
                item.Id = result.Id;
                item.Name = result.Person.FullName;
                item.Rank = result.Rank.Name;

                this.ViewModel.Items.Add(item);
                this.ViewModel.SelectedItem = item;

                //this.Search();
            }
        }

        public virtual void Edit(OfficerListItemViewModel item)
        {
            var dialog = new DialogService<OfficerView, OfficerViewModel>();
            dialog.ViewModel.Load.Execute(item.Id);
            var result = dialog.ShowModal(this, "Edit Officer", null);
            if (result != null)
            {
                this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Officer"));

                item.Id = result.Id;
                item.Name = result.Person.FullName;
                item.Rank = result.Rank.Name;

                //this.Search();
            }
        }

        public virtual void Delete(OfficerListItemViewModel item)
        {
            try
            {
                var message = string.Format("Are you sure you want to delete officer {0} {1}.", item.Rank, item.Name);
                var confirmed = this.MessageBox.Confirm(message, "Delete");
                if (confirmed == false)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var officer = session.Load<Officer>(item.Id);

                    session.Delete(officer);
                    transaction.Commit();
                }

                this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Officer"));

                this.ViewModel.Items.Remove(item);
                this.ViewModel.SelectedItem = null;

            }
            catch (GenericADOException ex)
            {
                var message = string.Format("Unable to delete. Officer {0} may already be in use.", item.Name);
                this.MessageBox.Warn(message, ex, "Error");
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex, "Error");
            }
        }
    }
}
