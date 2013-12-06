using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Firearms;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Maintenances
{
    [HandleError]
    public class MakeListController : ControllerBase<MakeListViewModel>
    {
        public MakeListController(MakeListViewModel viewModel)
            : base(viewModel)
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
            this.ViewModel.Load.Subscribe(x => Populate());
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
            this.ViewModel.Delete.Subscribe(x => Delete((MakeViewModel)x));
            this.ViewModel.Delete.ThrownExceptions.Handle(this);

            this.ViewModel.Search = new ReactiveCommand(
                this.ViewModel.WhenAny(
                    x => x.NewItem, 
                    x => !string.IsNullOrWhiteSpace(x.Value)
                )
            );
            this.ViewModel.Search.Subscribe(x => Search());
            this.ViewModel.Search.ThrownExceptions.Handle(this);

            this.Populate();
        }

        public virtual void Populate()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Make>().Cacheable().ToList();
                this.ViewModel.Items = query
                    .Select(x => new MakeViewModel(x.Id, x.Name))
                    .OrderBy(x => x.Name)
                    .ToReactiveList();

                transaction.Commit();
            }
        }

        public virtual void Insert()
        {
            var message = string.Format("Do you want to insert {0}?", this.ViewModel.NewItem);
            var confirmed = this.MessageBox.Confirm(message, "Save");
            if (confirmed == false)
                return;

            var newlyCreatedItem = (MakeViewModel)null;
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var exists = session.Query<Make>().Any(x => x.Name == this.ViewModel.NewItem);
                if (exists)
                {
                    this.MessageBox.Warn("Item already exists", "Manufacture (Make)");
                    return;
                }

                var entity = new Make() { Name = this.ViewModel.NewItem };

                session.Save(entity);
                transaction.Commit();

                newlyCreatedItem = new MakeViewModel(entity.Id, entity.Name);
            }

            this.ViewModel.Items.Insert(0, newlyCreatedItem);
            this.ViewModel.SelectedItem = newlyCreatedItem;
            this.ViewModel.NewItem = string.Empty;
        }

        public virtual void Delete(MakeViewModel item)
        {
            var message = string.Format("Do you want to delete {0}?", item.Name);
            var confirmed = this.MessageBox.Confirm(message, "Delete");
            if (confirmed == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entity = session.Get<Make>(item.Id);

                session.Delete(entity);
                transaction.Commit();
            }

            this.ViewModel.Items.Remove(item);
            this.ViewModel.SelectedItem = null;
        }

        public virtual void Search()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Make>()
                    .Where(x => x.Name == this.ViewModel.NewItem)
                    .ToFuture();

                this.ViewModel.Items = query
                    .Select(x => new MakeViewModel(x.Id, x.Name))
                    .OrderBy(x => x.Name)
                    .ToReactiveList();

                transaction.Commit();
            }
        }
    }
}
