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

namespace CIS.UI.Features.Firearms.Maintenances
{
    public class MakeListController : ControllerBase<MakeListViewModel>
    {
        public MakeListController(MakeListViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.ObservableForProperty(x => x.NewItem)
                .Subscribe(x =>
                {
                    var matchedItem = this.ViewModel.Items
                        .Where(o => o.Name.Contains(this.ViewModel.NewItem))
                        .FirstOrDefault();

                    this.ViewModel.SelectedItem = matchedItem;
                });

            this.ViewModel.Load = new ReactiveCommand();
            this.ViewModel.Load.Subscribe(x => Populate());

            this.ViewModel.Insert = new ReactiveCommand(this.ViewModel
                .WhenAny(
                    x => x.NewItem,
                    x =>
                        !string.IsNullOrWhiteSpace(x.Value) &&
                        !this.ViewModel.Items.Any(o => o.Name == x.Value)
                )
            );
            this.ViewModel.Insert.Subscribe(x => Insert());

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((MakeViewModel)x));

            this.ViewModel.Search = new ReactiveCommand(this.ViewModel
                .WhenAny(
                    x => x.NewItem,
                    x => !string.IsNullOrWhiteSpace(x.Value)
                )
            );
            this.ViewModel.Search.Subscribe(x => Search());

            this.Populate();
        }

        public virtual void Populate()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Make>().ToFuture();
                this.ViewModel.Items = query.Select(x => new MakeViewModel(x.Id, x.Name)).ToReactiveList();

                transaction.Commit();
            }
        }

        public virtual void Insert()
        {
            var message = string.Format("Do you want to insert {0}?", this.ViewModel.NewItem);
            var confirm = this.MessageBox.Confirm(message, "Manufacture (Make)");
            if (confirm == false)
                return;

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

                var newlyCreatedItem = new MakeViewModel(entity.Id, entity.Name);
                this.ViewModel.Items.Insert(0, newlyCreatedItem);
                this.ViewModel.SelectedItem = newlyCreatedItem;
                this.ViewModel.NewItem = string.Empty;
            }
        }

        public virtual void Delete(MakeViewModel item)
        {
            var message = string.Format("Do you want to delete {0}?", item.Name);
            var confirm = this.MessageBox.Confirm(message, "Manufacture (Make)");
            if (confirm == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entity = session.Get<Make>(item.Id);

                session.Delete(entity);
                transaction.Commit();

                this.ViewModel.Items.Remove(item);
                this.ViewModel.SelectedItem = null;
            }
        }

        public virtual void Search()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Make>().Where(x => x.Name == this.ViewModel.NewItem).ToFuture();
                this.ViewModel.Items = query.Select(x => new MakeViewModel(x.Id, x.Name)).ToReactiveList();

                transaction.Commit();
            }
        }
    }
}
