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
    public class KindListController : ControllerBase<KindListViewModel>
    {
        public KindListController(KindListViewModel viewModel)
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
            this.ViewModel.Delete.Subscribe(x => Delete((KindViewModel)x));

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
                var kindQuery = session.Query<Kind>()
                    .ToFuture();

                this.ViewModel.Items = kindQuery
                    .Select(x => new KindViewModel()
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
            var confirm = MessageDialog.Show(message, "Classification (Kind)", MessageBoxButton.YesNo);
            if (confirm == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var exists = session.Query<Kind>().Any(x => x.Name == this.ViewModel.NewItem);
                if (exists)
                {
                    MessageDialog.Show("Item already exists", "Classification (Kind)", MessageBoxButton.YesNo);
                    return;
                }

                var kind = new Kind() { Name = this.ViewModel.NewItem };

                session.Save(kind);
                transaction.Commit();

                var newlyCreatedItem = new KindViewModel() { Id = kind.Id, Name = kind.Name };
                this.ViewModel.Items.Insert(0, newlyCreatedItem);
                this.ViewModel.SelectedItem = newlyCreatedItem;
                this.ViewModel.NewItem = string.Empty;
            }
        }

        public virtual void Delete(KindViewModel item)
        {
            var message = string.Format("Do you want to delete {0}?", item.Name);
            var confirm = MessageDialog.Show(message, "Classification (Kind)", MessageBoxButton.YesNo);
            if (confirm == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var kind = session.Get<Kind>(item.Id);

                session.Delete(kind);
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
                var kindQuery = session.Query<Kind>()
                    .Where(x => x.Name == this.ViewModel.NewItem)
                    .ToFuture();

                this.ViewModel.Items = kindQuery
                    .Select(x => new KindViewModel()
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
