using System;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Polices.Clearances.Applications;

public class OtherInformationController : ControllerBase<OtherInformationViewModel>
{
    public OtherInformationController(OtherInformationViewModel viewModel)
        : base(viewModel)
    {
        this.ViewModel.EditFather = new ReactiveCommand();
        this.ViewModel.EditFather.Subscribe(x => this.EditFather());
        this.ViewModel.EditFather.ThrownExceptions.Handle(this);

        this.ViewModel.EditMother = new ReactiveCommand();
        this.ViewModel.EditMother.Subscribe(x => this.EditMother());
        this.ViewModel.EditMother.ThrownExceptions.Handle(this);

        this.ViewModel.CreateRelative = new ReactiveCommand();
        this.ViewModel.CreateRelative.Subscribe(x => this.CreateRelative());
        this.ViewModel.CreateRelative.ThrownExceptions.Handle(this);

        this.ViewModel.EditRelative = new ReactiveCommand(this.ViewModel.WhenAny(x => x.SelectedRelative, x => x.Value != null));
        this.ViewModel.EditRelative.Subscribe(x => this.EditRelative((PersonBasicViewModel)x));
        this.ViewModel.EditRelative.ThrownExceptions.Handle(this);

        this.ViewModel.DeleteRelative = new ReactiveCommand(this.ViewModel.WhenAny(x => x.SelectedRelative, x => x.Value != null));
        this.ViewModel.DeleteRelative.Subscribe(x => this.DeleteRelative((PersonBasicViewModel)x));
        this.ViewModel.DeleteRelative.ThrownExceptions.Handle(this);
    }

    public virtual void EditFather()
    {
        var dialog = new DialogService<PersonBasicDialogViewModel>();
        dialog.ViewModel.Person.SerializeWith(this.ViewModel.Father);
        dialog.ViewModel.Accept = new ReactiveCommand(dialog.ViewModel.Person.IsValidObservable());
        dialog.ViewModel.Accept.Subscribe(x => UpdateFather(dialog.ViewModel.Person));
        dialog.ViewModel.Accept.ThrownExceptions.Handle(this);
        dialog.ShowModal(this, "Father");
    }

    public virtual void UpdateFather(PersonBasicViewModel value)
    {
        this.ViewModel.Father.SerializeWith(value);
        value.Close();
    }

    public virtual void EditMother()
    {
        var dialog = new DialogService<PersonBasicDialogViewModel>();
        dialog.ViewModel.Person.SerializeWith(this.ViewModel.Mother);
        dialog.ViewModel.Accept = new ReactiveCommand(dialog.ViewModel.Person.IsValidObservable());
        dialog.ViewModel.Accept.Subscribe(x => UpdateMother(dialog.ViewModel.Person));
        dialog.ViewModel.Accept.ThrownExceptions.Handle(this);
        dialog.ShowModal(this, "Mother");
    }

    public virtual void UpdateMother(PersonBasicViewModel value)
    {
        this.ViewModel.Mother.SerializeWith(value);
        value.Close();
    }

    public virtual void CreateRelative()
    {
        var dialog = new DialogService<PersonBasicDialogViewModel>();
        dialog.ViewModel.Accept = new ReactiveCommand(dialog.ViewModel.Person.IsValidObservable());
        dialog.ViewModel.Accept.Subscribe(x => InsertRelative(dialog.ViewModel.Person));
        dialog.ViewModel.Accept.ThrownExceptions.Handle(this);
        dialog.ShowModal(this, "Relative");
    }

    public virtual void InsertRelative(PersonBasicViewModel value)
    {
        this.ViewModel.Relatives.Add(value);
        this.ViewModel.SelectedRelative = value;
        value.Close();
    }

    public virtual void EditRelative(PersonBasicViewModel value)
    {
        this.ViewModel.SelectedRelative = value;

        var dialog = new DialogService<PersonBasicDialogViewModel>();
        dialog.ViewModel.Person.SerializeWith(this.ViewModel.SelectedRelative);
        dialog.ViewModel.Accept = new ReactiveCommand(dialog.ViewModel.Person.IsValidObservable());
        dialog.ViewModel.Accept.Subscribe(x => UpdateRelative(dialog.ViewModel.Person));
        dialog.ViewModel.Accept.ThrownExceptions.Handle(this);
        dialog.ShowModal(this, "Relative");
    }

    public virtual void UpdateRelative(PersonBasicViewModel value)
    {
        this.ViewModel.SelectedRelative.SerializeWith(value);
        value.Close();
    }

    public virtual void DeleteRelative(PersonBasicViewModel value)
    {
        this.ViewModel.SelectedRelative = value;

        this.ViewModel.Relatives.Remove(this.ViewModel.SelectedRelative);
        this.ViewModel.SelectedRelative = null;
    }
}
