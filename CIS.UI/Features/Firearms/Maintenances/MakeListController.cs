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
            this.ViewModel.Load.Subscribe(x => Load());

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

            Load();
        }

        public virtual void Load()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var makeQuery = session.Query<Make>()
                    .ToFuture();

                this.ViewModel.Items = makeQuery
                    .Select(x => new MakeViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToReactiveColletion();

                transaction.Commit();
            }
        }

        public virtual void Insert()
        {
            var message = string.Format("Do you want to insert new {0}?", this.ViewModel.NewItem);
            var confirm = MessageDialog.Show(message, "Manufacture (Make)", MessageBoxButton.YesNo);
            if (confirm == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var exists = session.Query<Make>().Any(x => x.Name == this.ViewModel.NewItem);
                if (exists)
                {
                    MessageDialog.Show("Item already exists", "Manufacture (Make)", MessageBoxButton.YesNo);
                    return;
                }

                var make = new Make() { Name = this.ViewModel.NewItem };

                session.Save(make);
                transaction.Commit();

                var newlyCreatedItem = new MakeViewModel() { Id = make.Id, Name = make.Name };
                this.ViewModel.Items.Insert(0, newlyCreatedItem);
                this.ViewModel.SelectedItem = newlyCreatedItem;
                this.ViewModel.NewItem = string.Empty;
            }
        }

        public virtual void Delete(MakeViewModel item)
        {
            var message = string.Format("Do you want to delete {0}?", item.Name);
            var confirm = MessageDialog.Show(message, "Manufacture (Make)", MessageBoxButton.YesNo);
            if (confirm == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var make = session.Get<Make>(item.Id);

                session.Delete(make);
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
                var makeQuery = session.Query<Make>()
                    .Where(x => x.Name == this.ViewModel.NewItem)
                    .ToFuture();

                this.ViewModel.Items = makeQuery
                    .Select(x => new MakeViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToReactiveColletion();

                transaction.Commit();
            }
        }
    }
}
