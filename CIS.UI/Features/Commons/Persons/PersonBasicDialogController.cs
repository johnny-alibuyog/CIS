using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using CIS.UI.Utilities.Extentions;

namespace CIS.UI.Features.Commons.Persons
{
    public class PersonBasicDialogController : ControllerBase<PersonBasicDialogViewModel>
    {
        public PersonBasicDialogController(PersonBasicDialogViewModel viewModel) : base(viewModel)
        {
            //this.ViewModel.Accept = new ReactiveCommand(this.ViewModel.IsValidObservable());
            //this.ViewModel.Accept.Subscribe(x => this.ViewModel.Close());
            //this.ViewModel.Accept.ThrownExceptions.Handle(this);
        }
    }
}
