using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Polices;
using CIS.UI.Utilities.CommonDialogs;
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
            var result = dialog.ShowModal(this, "Create Officer", null);
            if (result != null)
            {
                this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Officer"));
                this.Search();
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
                this.Search();
            }
        }

        public virtual void Delete(OfficerListItemViewModel item)
        {
            try
            {
                var question = string.Format("Are you sure you want to delete officer {0} {1}.", item.Rank, item.Name);
                var result = this.Confirm(question, "Delete");
                if (result == false)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var officer = session.Load<Officer>(item.Id);

                    session.Delete(officer);
                    transaction.Commit();
                }

                this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Officer"));
            }
            catch (GenericADOException)
            {
                var message = string.Format("Unable to delete. Officer {0} may already be in use.", item.Name);
                this.Warn(message, "Error");
            }
            catch (Exception ex)
            {
                this.Warn(ex.Message, "Error");
            }
        }
    }
}
