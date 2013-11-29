using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Persons;
using ReactiveUI;

namespace CIS.UI.Features.Polices.Clearances
{
    public class OtherInformationViewModel : ViewModelBase
    {
        private readonly OtherInformationController _controller;

        public virtual PersonBasicViewModel Father { get; set; }

        public virtual PersonBasicViewModel Mother { get; set; }

        public virtual IReactiveList<PersonBasicViewModel> Relatives { get; set; }

        public virtual AddressViewModel ProvincialAddress { get; set; }

        public virtual PersonBasicViewModel SelectedRelative { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual string TelephoneNumber { get; set; }

        public virtual string CellphoneNumber { get; set; }

        public virtual string PassportNumber { get; set; }

        public virtual string TaxIdentificationNumber { get; set; }

        public virtual string SocialSecuritySystemNumber { get; set; }

        public virtual IReactiveCommand EditFather { get; set; }

        public virtual IReactiveCommand EditMother { get; set; }

        public virtual IReactiveCommand CreateRelative { get; set; }

        public virtual IReactiveCommand EditRelative { get; set; }

        public virtual IReactiveCommand DeleteRelative { get; set; }

        public OtherInformationViewModel()
        {
            this.Father = new PersonBasicViewModel();
            this.Mother = new PersonBasicViewModel();
            this.Relatives = new ReactiveList<PersonBasicViewModel>();
            this.ProvincialAddress = new AddressViewModel();

            _controller = IoC.Container.Resolve<OtherInformationController>(new ViewModelDependency(this));
        }
    }
}
