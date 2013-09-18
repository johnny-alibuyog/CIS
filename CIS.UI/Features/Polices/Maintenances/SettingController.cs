﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Polices;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
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
            this.Load();

            this.ViewModel.Load = new ReactiveCommand();
            this.ViewModel.Load.Subscribe(x => Load());

            this.ViewModel.Save = new ReactiveCommand();
            this.ViewModel.Save.Subscribe(x => Save());
        }

        [HandleError]
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

        [HandleError]
        public virtual void Save()
        {
            var confirmed = this.MessageBox.Confirm("Do you want to save changes?", "Save");
            if (confirmed == false)
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
