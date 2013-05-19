﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Polices;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Utilities.CommonDialogs;
using NHibernate;
using NHibernate.Context;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Stations
{
    public class StationController : ControllerBase<StationViewModel>
    {
        public StationController(StationViewModel viewModel)
            : base(viewModel)
        {
            Load();

            this.ViewModel.LookupLogo = new ReactiveCommand();
            this.ViewModel.LookupLogo.Subscribe(x => LookupLogo());

            this.ViewModel.Save = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value));
            this.ViewModel.Save.Subscribe(x => Save());

            this.ViewModel.Refresh = new ReactiveCommand();
            this.ViewModel.Refresh.Subscribe(x => Load(confirm: true));
        }

        public virtual void LookupLogo()
        {
            var logo = OpenImageDialog.Show();
            if (logo != null)
                this.ViewModel.Logo = logo;
        }

        public virtual void Load(bool confirm = false)
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Station>()
                    .Fetch(x => x.Logo)
                    .ThenFetch(x => x.Image)
                    .ToFuture();

                var station = query.FirstOrDefault();
                if (station != null)
                    this.ViewModel.SerializeWith(station);

                transaction.Commit();
            }

            if (confirm)
                MessageDialog.Show("Station configuration has loaded.", "Station", MessageBoxButton.OK);
        }

        public virtual void Save()
        {
            var confirm = MessageDialog.Show("Do you want to save changes?.", "Station", MessageBoxButton.YesNo);
            if (confirm == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Station>()
                    .Fetch(x => x.Logo)
                    .ThenFetch(x => x.Image)
                    .ToFuture();

                var station = query.FirstOrDefault();
                if (station == null)
                    station = new Station();

                this.ViewModel.SerializeInto(station);

                session.SaveOrUpdate(station);
                transaction.Commit();
            }

            MessageDialog.Show("Station configuration has been saved.", "Station", MessageBoxButton.OK);
        }
    }
}
