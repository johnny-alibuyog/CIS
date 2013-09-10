using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Firearms;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Utilities.CommonDialogs;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class LicenseContoller : ControllerBase<LicenseViewModel>
    {
        public LicenseContoller(LicenseViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.IssueDate = DateTime.Today;
            this.ViewModel.ExpiryDate = DateTime.Today;

            this.ViewModel.Person = new PersonViewModel();
            this.ViewModel.Address = new AddressViewModel();
            this.ViewModel.Gun = new GunViewModel();

            this.ViewModel.Load = new ReactiveCommand();
            this.ViewModel.Load.Subscribe(x => Load((Guid)x));

            this.ViewModel.Save = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value));
            this.ViewModel.Save.Subscribe(x => Save());
        }

        public virtual void Load(Guid id)
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<License>()
                    .Where(x => x.Id == id)
                    .ToFutureValue();

                var license = query.Value;

                this.ViewModel.SerializeWith(license);

                transaction.Commit();
            }
        }

        public virtual void Save()
        {
            var message = string.Format("Are you sure you want to save warrant?");
            var confirm = this.MessageBox.Confirm(message, "Save");
            if (confirm == false)
                return;

            using (var session = this.SessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var license = (License)null;

                if (this.ViewModel.Id == Guid.Empty)
                    license = new License();
                else
                    license = session.Get<License>(this.ViewModel.Id);

                this.ViewModel.SerializeInto(license);

                session.SaveOrUpdate(license);
                transaction.Commit();

                this.ViewModel.Id = license.Id;

                this.SessionProvider.ReleaseSharedSession();
            }

            this.ViewModel.ActionResult = true;
        }
    }
}
