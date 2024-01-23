using System;
using System.Collections.Generic;
using System.Linq;
using CIS.UI.Bootstraps.InversionOfControl;
using FirstFloor.ModernUI.Presentation;
using NHibernate.Linq;

namespace CIS.UI.Features;

public class MainController : ControllerBase<MainViewModel>
{
    private IEnumerable<ITaskScheduler> _tasks;

    public MainController(MainViewModel viewModel)
        : base(viewModel)
    {
        this.InitializeViewModel();
        this.InitializeTaskSchedulers();
    }

    private void InitializeViewModel()
    {
        this.ViewModel.LinkGroups = [];

        if (App.Data.User.IsPoliceStaff())
        {
            addLinkGroup("police",
                new Link() { DisplayName = "Clearance", Source = new Uri("/Features/Polices/Clearances/ClearancePage.xaml", UriKind.Relative) },
                new Link() { DisplayName = "Warrant", Source = new Uri("/Features/Polices/Warrants/WarrantPage.xaml", UriKind.Relative) },
                new Link() { DisplayName = "Maintenance", Source = new Uri("/Features/Polices/Maintenances/MaintenancePage.xaml", UriKind.Relative) }
            );
        }

        if (App.Data.User.IsBarangayStaff())
        {
            addLinkGroup("barangay",
                new Link() { DisplayName = "Clearance", Source = new Uri("/Features/Barangays/Clearances/ClearancePage.xaml", UriKind.Relative) },
                new Link() { DisplayName = "Blotter", Source = new Uri("/Features/Barangays/Blotters/BlotterPage.xaml", UriKind.Relative) },
                new Link() { DisplayName = "Maintenance", Source = new Uri("/Features/Barangays/Maintenances/MaintenancePage.xaml", UriKind.Relative) }
            );
        }

        if (App.Data.User.IsMayorStaff())
        {
            addLinkGroup("mayor",
                new Link() { DisplayName = "Clearance" },
                new Link() { DisplayName = "Maintenance" }
            );  
        }

        addLinkGroup("membership",
            new Link() { DisplayName = "User", Source = new Uri("/Features/Memberships/Users/UserPage.xaml", UriKind.Relative) }
        );

        addLinkGroup("settings",
            new Link() { DisplayName = "software", Source = new Uri("/Features/Settings/Page.xaml", UriKind.Relative) }
        );

        void addLinkGroup(string displayName, params Link[] links)
        {
            var group = new LinkGroup() { DisplayName = displayName };
            links.ForEach(group.Links.Add);

            this.ViewModel.LinkGroups.Add(group);
            this.ViewModel.DefaultPage ??= group.Links.Select(x => x.Source).FirstOrDefault();
        }
    }

    private void InitializeTaskSchedulers()
    {
        _tasks = IoC.Container.ResolveAll<ITaskScheduler>();
        if (App.Config.SyncronizeToDataStore)
        {
            foreach (var task in _tasks)
            {
                task.StartWork();
            }
        }
    }
}
