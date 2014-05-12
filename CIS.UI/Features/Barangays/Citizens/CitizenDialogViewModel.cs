using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate.Validator.Constraints;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Citizens
{
    public class CitizenDialogViewModel : ViewModelBase
    {
        private readonly CitizenDialogController _controller;

        [Valid]
        public virtual CitizenViewModel Citizen { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public CitizenDialogViewModel()
        {
            Citizen = new CitizenViewModel();

            _controller = IoC.Container.Resolve<CitizenDialogController>(new ViewModelDependency(this));
        }

    }
}
