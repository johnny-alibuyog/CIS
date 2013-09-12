using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Commons.Signatures
{
    public class SignatureDialogViewModel : ViewModelBase
    {
        private readonly SignatureDialogController _controller;

        [Valid]
        public virtual SignatureViewModel Signature { get; set; }

        public virtual IReactiveCommand Accept { get; set; }

        public SignatureDialogViewModel()
        {
            Signature = new SignatureViewModel();

            //_controller = new SignatureDialogController(this);
            _controller = IoC.Container.Resolve<SignatureDialogController>(new ViewModelDependency(this));
        }
    }
}
