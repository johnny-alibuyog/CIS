using System;
using System.Linq;
using System.Reactive.Linq;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Common.Wizard;

public class WizardController : ControllerBase<WizardViewModel>
{
    public WizardController(WizardViewModel viewModel) : base(viewModel)
    {
        this.ViewModel.CurrentStep = this.ViewModel.Steps.First();

        this.ViewModel.ResetCommand = new ReactiveCommand(this.CanReset);
        this.ViewModel.ResetCommand.Subscribe(x => this.Reset());
        this.ViewModel.ResetCommand.ThrownExceptions.Handle(this);

        this.ViewModel.BackCommand = new ReactiveCommand(this.CanGoBack);
        this.ViewModel.BackCommand.Subscribe(x => this.Back());
        this.ViewModel.BackCommand.ThrownExceptions.Handle(this);

        this.ViewModel.NextCommand = new ReactiveCommand(this.CanGoNext);
        this.ViewModel.NextCommand.Subscribe(x => this.Next());
        this.ViewModel.NextCommand.ThrownExceptions.Handle(this);

        this.ViewModel.SubmitCommand = new ReactiveCommand(this.CanSubmit);
        this.ViewModel.SubmitCommand.Subscribe(x => this.Submit());
        this.ViewModel.SubmitCommand.ThrownExceptions.Handle(this);
    }

    private WizardStep NextStep
    {
        get => this.ViewModel.Steps
            .SkipWhile(x => x != this.ViewModel.CurrentStep)
            .Skip(1)
            .FirstOrDefault();
    }

    private WizardStep PreviousStep
    {
        get => this.ViewModel.Steps
            .TakeWhile(x => x != this.ViewModel.CurrentStep)
            .LastOrDefault();
    }

    private WizardStep FirstStep
    {
        get => this.ViewModel.Steps.First();
    }

    private WizardStep LastStep
    {
        get => this.ViewModel.Steps.Last();
    }

    private IObservable<bool> CanReset
    {
        get => Observable.Return(true);
    }

    private IObservable<bool> CanGoBack
    {
        get => this.ViewModel.WhenAnyValue(
            x => x.CurrentStep,
            x => x != this.FirstStep
        );
    }

    private IObservable<bool> CanGoNext
    {
        get => this.ViewModel.WhenAnyValue(
            x => x.CurrentStep,
            x => x.CurrentStep.IsValid,
            (currentStep, isValid) => currentStep != this.LastStep && isValid
        );
    }

    private IObservable<bool> CanSubmit
    {
        get => this.ViewModel.WhenAnyValue(
            x => x.CurrentStep,
            x => x.CurrentStep.IsValid,
            (currentStep, isValid) => currentStep == this.LastStep && isValid
        );

    }

    private void Reset()
    {
        this.ViewModel.OnReset?.Invoke();
    }

    private void Back()
    {
        this.ViewModel.CurrentStep.Unload();
        this.ViewModel.CurrentStep = this.PreviousStep;
        this.ViewModel.CurrentStep.Load();
    }

    private void Next()
    {
        this.ViewModel.CurrentStep.Unload();
        this.ViewModel.CurrentStep = this.NextStep;
        this.ViewModel.CurrentStep.Load();
    }

    private void Submit()
    {
        this.ViewModel.OnSubmit?.Invoke();
    }
}
