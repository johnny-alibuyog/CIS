using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using CIS.Core.Entities.Polices;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Maintenances.Purposes
{
    [HandleError]
    public class PurposeListController : ControllerBase<PurposeListViewModel>
    {
        public PurposeListController(PurposeListViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.ObservableForProperty(x => x.NewItem)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Subscribe(x =>
                {
                    var matchedItem = this.ViewModel.Items
                        .Where(o => o.Name.ToLower().Contains(x.Value.ToLower()))
                        .FirstOrDefault();

                    this.ViewModel.SelectedItem = matchedItem;
                });

            this.ViewModel.Load = new ReactiveCommand();
            this.ViewModel.Load.Subscribe(x => Load());
            this.ViewModel.Load.ThrownExceptions.Handle(this);

            this.ViewModel.Insert = new ReactiveCommand(
                this.ViewModel.WhenAny(
                    x => x.NewItem,
                    x =>
                        !string.IsNullOrWhiteSpace(x.Value) &&
                        !this.ViewModel.Items.Any(o => o.Name == x.Value)
                )
            );
            this.ViewModel.Insert.Subscribe(x => Insert());
            this.ViewModel.Insert.ThrownExceptions.Handle(this);

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((PurposeViewModel)x));
            this.ViewModel.Delete.ThrownExceptions.Handle(this);

            this.ViewModel.Search = new ReactiveCommand(
                this.ViewModel.WhenAny(
                    x => x.NewItem,
                    x => !string.IsNullOrWhiteSpace(x.Value)
                )
            );
            this.ViewModel.Search.Subscribe(x => Search());
            this.ViewModel.Search.ThrownExceptions.Handle(this);

            this.Load();
        }

        public virtual void Load()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Purpose>()
                    .ToFuture();

                this.ViewModel.Items = query
                    .Select(x => new PurposeViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .OrderBy(x => x.Name)
                    .ToReactiveList();

                transaction.Commit();
            }
        }

        public virtual void Insert()
        {
            var message = string.Format("Do you want to save {0}?", this.ViewModel.NewItem);
            var confirmed = this.MessageBox.Confirm(message, "Save");
            if (confirmed == false)
                return;

            var newlyCreatedItem = (PurposeViewModel)null;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var exists = session.Query<Purpose>().Any(x => x.Name == this.ViewModel.NewItem);
                if (exists)
                {
                    this.MessageBox.Inform("Item already exists", "Purpose");
                    return;
                }

                var entity = new Purpose() { Name = this.ViewModel.NewItem };

                session.Save(entity);
                transaction.Commit();

                newlyCreatedItem = new PurposeViewModel() { Id = entity.Id, Name = entity.Name };
            }

            this.ViewModel.Items.Insert(0, newlyCreatedItem);
            this.ViewModel.SelectedItem = newlyCreatedItem;
            this.ViewModel.NewItem = string.Empty;


            this.MessageBox.Inform("Save has been successfully completed.");
            //this.MessageBox.Inform("Purpose has been added.", "Station");

            this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Purpose"));
        }

        public virtual void Delete(PurposeViewModel item)
        {
            var message = string.Format("Do you want to delete {0}?", item.Name);
            var confirmed = this.MessageBox.Confirm(message, "Purpose");
            if (confirmed == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entity = session.Get<Purpose>(item.Id);

                session.Delete(entity);
                transaction.Commit();
            }

            this.ViewModel.Items.Remove(item);
            this.ViewModel.SelectedItem = null;

            this.MessageBox.Inform("Delete has been successfully completed.");
            //this.MessageBox.Inform("Purpose has been deleted.", "Purpose");
            
            this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Purpose"));
        }

        public virtual void Search()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Purpose>()
                    .Where(x => x.Name == this.ViewModel.NewItem)
                    .ToFuture();

                this.ViewModel.Items = query
                    .Select(x => new PurposeViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .OrderBy(x => x.Name)
                    .ToReactiveList();

                transaction.Commit();
            }
        }
    }
}
