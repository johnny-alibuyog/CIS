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

        public virtual BasicPersonViewModel Father { get; set; }

        public virtual BasicPersonViewModel Mother { get; set; }

        public virtual IReactiveList<BasicPersonViewModel> Relatives { get; set; }

        public virtual AddressViewModel ProvincialAddress { get; set; }

        public virtual BasicPersonViewModel SelectedRelative { get; set; }

        public virtual IReactiveCommand EditFather { get; set; }

        public virtual IReactiveCommand EditMother { get; set; }

        public virtual IReactiveCommand CreateRelative { get; set; }

        public virtual IReactiveCommand EditRelative { get; set; }

        public virtual IReactiveCommand DeleteRelative { get; set; }

        public OtherInformationViewModel()
        {
            this.Father = new BasicPersonViewModel();
            this.Mother = new BasicPersonViewModel();
            this.Relatives = new ReactiveList<BasicPersonViewModel>();
            this.ProvincialAddress = new AddressViewModel();

            _controller = IoC.Container.Resolve<OtherInformationController>(new ViewModelDependency(this));
        }
    }
}
