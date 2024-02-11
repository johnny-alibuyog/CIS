namespace CIS.UI.Features.Common.Wizard;

public abstract class WizardStep : ViewModelBase
{
    public abstract string Title { get; set; }
    public abstract void Reset();
    public abstract void Load();
    public abstract void Unload();
}
