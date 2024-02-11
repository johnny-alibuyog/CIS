using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Common.Wizard;

/// <summary>
/// Interaction logic for ApplicationView.xaml
/// </summary>
public partial class WizardView : UserControl, IViewFor<WizardViewModel>
{
    //public static readonly DependencyProperty StepsProperty = DependencyProperty.Register("Steps", typeof(IEnumerable<WizardStep>), typeof(WizardView));
    //public static readonly DependencyProperty OnResetProperty = DependencyProperty.Register("OnReset", typeof(Action), typeof(WizardView));
    //public static readonly DependencyProperty OnSubmitProperty = DependencyProperty.Register("OnSubmit", typeof(Action), typeof(WizardView));

    //public IEnumerable<WizardStep> Steps
    //{
    //    get => this.GetValue(StepsProperty) as IEnumerable<WizardStep>;
    //    set => this.SetValue(StepsProperty, value);
    //}

    //public Action OnReset
    //{
    //    get => this.GetValue(OnResetProperty) as Action;
    //    set => this.SetValue(OnResetProperty, value);
    //}

    //public Action OnSubmit
    //{
    //    get => this.GetValue(OnSubmitProperty) as Action;
    //    set => this.SetValue(OnSubmitProperty, value);
    //}

    public WizardViewModel ViewModel
    {
        get => this.DataContext as WizardViewModel;
        set => this.DataContext = value;
    }

    object IViewFor.ViewModel
    {
        get => this.DataContext;
        set => this.DataContext = value;
    }

    //public override void EndInit()
    //{
    //    base.EndInit();
    //    //this.InitializeViewModelAsync(() => new WizardViewModel(this.Steps, this.OnReset, this.OnSubmit));
    //    //this.ViewModel = new(this.Steps, this.OnReset, this.OnSubmit);

    //    //    this.WhenAnyValue(x => x.ViewModel).WhereNotNull().Subscribe(vm =>
    //    //    {
    //    //        this.BindCommand(this.ViewModel, vm => vm.Next, v => v.NextButton);
    //    //        this.BindCommand(this.ViewModel, vm => vm.Previous, v => v.PreviousButton);
    //    //        this.BindCommand(this.ViewModel, vm => vm.Reset, v => v.ResetButton);
    //    //        this.BindCommand(this.ViewModel, vm => vm.Submit, v => v.SubmitButton);
    //    //    });
    //    //}
    //}

    //public WizardView()
    //{
    //    //this.InitializeViewModelAsync(() =>
    //    //{
    //    //    return new WizardViewModel(this.Steps, this.OnReset, this.OnSubmit);
    //    //    //return IoC.Container.Resolve<WizardViewModel>([
    //    //    //    new("steps", this.Steps),
    //    //    //    new("onReset", this.OnReset),
    //    //    //    new("onSubmit", this.OnSubmit)
    //    //    //]);
    //    //});
    //}
}
