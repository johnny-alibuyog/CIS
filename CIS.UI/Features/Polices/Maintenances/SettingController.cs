using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Polices;
using CIS.UI.Utilities.CommonDialogs;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Maintenances
{
    public class SettingController : ControllerBase<SettingViewModel>
    {
        public SettingController(SettingViewModel viewModel) : base(viewModel)
        {
            Load();

            this.ViewModel.Load = new ReactiveCommand();
            this.ViewModel.Load.Subscribe(x => Load());

            this.ViewModel.Save = new ReactiveCommand();
            this.ViewModel.Save.Subscribe(x => Save());
        }

        public virtual void Load()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Setting>()
                    .Where(x => x.Terminal.MachineName == Environment.MachineName)
                    .Fetch(x => x.FingersToScan)
                    .ToFutureValue();

                var setting = query.Value;

                this.ViewModel.SerializeWith(setting);
                transaction.Commit();
            }
        }

        public virtual void Save()
        {
            var confirm = MessageDialog.Show("Do you want to save changes?", "Settings", MessageBoxButton.YesNo);
            if (confirm == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Setting>()
                    .Where(x => x.Terminal.MachineName == Environment.MachineName)
                    .Fetch(x => x.FingersToScan)
                    .ToFutureValue();

                var setting = query.Value;

                this.ViewModel.SerializeInto(setting);
                transaction.Commit();
            }

            this.MessageBus.SendMessage<MaintenanceMessage>(new MaintenanceMessage("Setting"));
        }
    }
}
