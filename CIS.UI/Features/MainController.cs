using System;
using System.Linq;
using CIS.UI.Bootstraps.InversionOfControl;
using FirstFloor.ModernUI.Presentation;
using NHibernate.Linq;

namespace CIS.UI.Features;

public class MainController : ControllerBase<MainViewModel>
{
    public MainController(MainViewModel viewModel)
        : base(viewModel)
    {
        this.InitializeViewModel();
        this.InitializeTaskSchedulers();
    }

    private void InitializeViewModel()
    {
        this.ViewModel.LinkGroups = [];

        if (App.Context.User.IsPoliceStaff())
        {
            addLinkGroup("membership",
                new Link() { DisplayName = "Registration", Source = new Uri("/Features/Membership/Registration/RegistrationPage.xaml", UriKind.Relative) },
                new Link() { DisplayName = "Members", Source = new Uri("/Features/Membership/MembershipInfo/MemberPage.xaml", UriKind.Relative) },
                //new Link() { DisplayName = "Warrant", Source = new Uri("/Features/Membership/Warrants/WarrantPage.xaml", UriKind.Relative) },
                new Link() { DisplayName = "Maintenance", Source = new Uri("/Features/Membership/Maintenance/MaintenancePage.xaml", UriKind.Relative) }
            );
        }

        addLinkGroup("security",
            new Link() { DisplayName = "User", Source = new Uri("/Features/Security/Users/UserPage.xaml", UriKind.Relative) }
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
        var tasks = IoC.Container.ResolveAll<ITaskScheduler>();
        if (App.Config.SyncronizeToDataStore)
        {
            foreach (var task in tasks)
            {
                task.StartWork();
            }
        }
    }
}
