using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;
using NHibernate;
using NHibernate.Linq;
using CIS.Core.Entities.Barangays;
using CIS.Core.Entities.Commons;
using CIS.UI.Features.Commons.Signatures;
using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate.Exceptions;

namespace CIS.UI.Features.Barangays.Maintenances.Incumbents
{
    public class IncumbentListController : ControllerBase<IncumbentListViewModel>
    {
        public IncumbentListController(IncumbentListViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Refresh = new ReactiveCommand();
            this.ViewModel.Refresh.Subscribe(x => Refresh());
            this.ViewModel.Refresh.ThrownExceptions.Handle(this);

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());
            this.ViewModel.Create.ThrownExceptions.Handle(this);

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => Edit((IncumbentListItemViewModel)x));
            this.ViewModel.Edit.ThrownExceptions.Handle(this);

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((IncumbentListItemViewModel)x));
            this.ViewModel.Delete.ThrownExceptions.Handle(this);

            this.Refresh();
        }

        public virtual void Refresh()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Official>()
                    .Where(x =>
                        x.IsActive == true &&
                        x.Position == Position.BarangayCaptain
                    )
                    .Select(x => new IncumbentListItemViewModel()
                    {
                        Id = x.Incumbent.Id,
                        Year = x.Incumbent.Date.Value.Year,
                        Captain = string.Format("Brgy. Cpt. {0} {1}", x.Person.FirstName, x.Person.LastName)
                    })
                    .OrderByDescending(x => x.Year);

                var list = query.ToReactiveList();
                var current = list.FirstOrDefault();

                foreach (var item in list)
                {
                    if (item == current)
                        item.Term = "Current";
                    else
                        item.Term = item.Year.ToString();
                }

                this.ViewModel.Items = list;

                transaction.Commit();
            }
        }

        public virtual void Create()
        {
            var dialog = new DialogService<IncumbentViewModel>();
            var result = dialog.ShowModal(this, "Create Incumbent", null);
            if (result != null)
                this.Refresh();
        }

        public virtual void Edit(IncumbentListItemViewModel item)
        {
            var dialog = new DialogService<IncumbentViewModel>();
            dialog.ViewModel.Load.Execute(item.Id);
            var result = dialog.ShowModal(this, "Edit Incumbent", null);
            if (result != null)
                this.Refresh();

        }

        public virtual void Delete(IncumbentListItemViewModel item)
        {
            try
            {
                this.ViewModel.SelectedItem = item;
                var selected = this.ViewModel.SelectedItem;
                if (selected == null)
                    return;

                var message = string.Format("Do you want to delete {0} incumbency of {1}?", selected.Year, selected.Captain);
                var confirmed = this.MessageBox.Confirm(message, "Delete");
                if (confirmed == false)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Incumbent>()
                        .FetchMany(x => x.Officials)
                        .ToFutureValue();

                    var incumbent = query.Value;
                    if (incumbent != null)
                        session.Delete(incumbent);

                    transaction.Commit();
                }

                this.MessageBox.Inform("Delete has been successfully completed.");

                this.Refresh();
            }
            catch (GenericADOException)
            {
                throw new InvalidOperationException(string.Format("Unable to delete. Incubency of {0} on {1} may already be in use.", item.Captain, item.Year));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
