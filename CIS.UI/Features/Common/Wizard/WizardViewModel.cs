using System;
using System.Collections.Generic;
using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;

namespace CIS.UI.Features.Common.Wizard;

public class WizardViewModel : ViewModelBase
{
    private readonly WizardController _controller;

    public string Title
    { 
        get => this.CurrentStep.Title; 
        set => this.CurrentStep.Title = value; 
    }

    public IEnumerable<WizardStep> Steps { get; set; } = [];

    public WizardStep CurrentStep { get; set; }

    public IReactiveCommand ResetCommand { get; set; }

    public IReactiveCommand NextCommand { get; set; }

    public IReactiveCommand BackCommand { get; set; }

    public IReactiveCommand SubmitCommand { get; set; }

    public Action OnReset { get; set; }

    public Action OnSubmit { get; set; }

    public WizardViewModel(IEnumerable<WizardStep> steps, Action onReset, Action onSubmit)
    {
        this.Steps = steps;
        this.OnReset = onReset;
        this.OnSubmit = onSubmit;
        //this._controller = new(this);
        this._controller = IoC.Container.Resolve<WizardController>(new ViewModelDependency(this));
    }
}
