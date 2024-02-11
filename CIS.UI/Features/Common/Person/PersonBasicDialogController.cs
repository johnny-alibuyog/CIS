namespace CIS.UI.Features.Common.Person;

public class PersonBasicDialogController : ControllerBase<PersonBasicDialogViewModel>
{
    public PersonBasicDialogController(PersonBasicDialogViewModel viewModel) : base(viewModel)
    {
        //this.ViewModel.Accept = new ReactiveCommand(this.ViewModel.IsValidObservable());
        //this.ViewModel.Accept.Subscribe(x => this.ViewModel.Close());
        //this.ViewModel.Accept.ThrownExceptions.Handle(this);
    }
}
