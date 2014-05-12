using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Polices.Warrants;
using FirstFloor.ModernUI.Presentation;
using ReactiveUI;

namespace CIS.UI.Features
{
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
            this.ViewModel.LinkGroups = new LinkGroupCollection();

            Action<Uri> SetDefaultPage = (page) =>
            {
                if (this.ViewModel.DefaultPage == null)
                    this.ViewModel.DefaultPage = page;
            };

            var linkGroup = (LinkGroup)null;
            if (App.Data.User.IsPoliceStaff())
            {
                linkGroup = new LinkGroup();
                linkGroup.DisplayName = "police";
                linkGroup.Links.Add(new Link() { DisplayName = "Clearance", Source = new Uri("/Features/Polices/Clearances/ClearancePage.xaml", UriKind.Relative) });
                linkGroup.Links.Add(new Link() { DisplayName = "Warrant", Source = new Uri("/Features/Polices/Warrants/WarrantPage.xaml", UriKind.Relative) });
                linkGroup.Links.Add(new Link() { DisplayName = "Maintenance", Source = new Uri("/Features/Polices/Maintenances/MaintenancePage.xaml", UriKind.Relative) });
                this.ViewModel.LinkGroups.Add(linkGroup);

                SetDefaultPage(linkGroup.Links.Select(x => x.Source).FirstOrDefault());

                linkGroup = new LinkGroup();
                linkGroup.DisplayName = "firearms";
                linkGroup.Links.Add(new Link() { DisplayName = "License", Source = new Uri("/Features/Firearms/Licenses/LicensePage.xaml", UriKind.Relative) });
                linkGroup.Links.Add(new Link() { DisplayName = "Maintenance", Source = new Uri("/Features/Firearms/Maintenances/MaintenancePage.xaml", UriKind.Relative) });
                this.ViewModel.LinkGroups.Add(linkGroup);
            }

            if (App.Data.User.IsBarangayStaff())
            {
                linkGroup = new LinkGroup();
                linkGroup.DisplayName = "barangay";
                linkGroup.Links.Add(new Link() { DisplayName = "Clearance", Source = new Uri("/Features/Barangays/Clearances/ClearancePage.xaml", UriKind.Relative) });
                linkGroup.Links.Add(new Link() { DisplayName = "Blotter", Source = new Uri("/Features/Barangays/Blotters/BlotterPage.xaml", UriKind.Relative) });
                linkGroup.Links.Add(new Link() { DisplayName = "Maintenance", Source = new Uri("/Features/Barangays/Maintenances/MaintenancePage.xaml", UriKind.Relative) });
                this.ViewModel.LinkGroups.Add(linkGroup);

                SetDefaultPage(linkGroup.Links.Select(x => x.Source).FirstOrDefault());
            }

            if (App.Data.User.IsMayorStaff())
            {
                linkGroup = new LinkGroup();
                linkGroup.DisplayName = "mayor";
                linkGroup.Links.Add(new Link() { DisplayName = "Clearance" });
                linkGroup.Links.Add(new Link() { DisplayName = "Maintenance" });
                this.ViewModel.LinkGroups.Add(linkGroup);

                SetDefaultPage(linkGroup.Links.Select(x => x.Source).FirstOrDefault());
            }

            linkGroup = new LinkGroup();
            linkGroup.DisplayName = "membership";
            linkGroup.Links.Add(new Link() { DisplayName = "User", Source = new Uri("/Features/Memberships/Users/UserPage.xaml", UriKind.Relative) });
            this.ViewModel.LinkGroups.Add(linkGroup);

            SetDefaultPage(linkGroup.Links.Select(x => x.Source).FirstOrDefault());

            linkGroup = new LinkGroup();
            linkGroup.DisplayName = "settings";
            linkGroup.Links.Add(new Link() { DisplayName = "software", Source = new Uri("/Features/Settings/Page.xaml", UriKind.Relative) });
            this.ViewModel.LinkGroups.Add(linkGroup);

            SetDefaultPage(linkGroup.Links.Select(x => x.Source).FirstOrDefault());
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
}
