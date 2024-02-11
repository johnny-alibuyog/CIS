using System.Windows.Media;
using CIS.UI.Bootstraps.InversionOfControl;
using FirstFloor.ModernUI.Presentation;
using ReactiveUI;

namespace CIS.UI.Features.Settings.Appearance;

/// <summary>
/// A simple view model for configuring theme, font and accent colors.
/// </summary>
public class AppearanceViewModel : ViewModelBase
{
    private readonly AppearanceController _controller;

    public virtual Link SelectedTheme { get; set; }

    public virtual Color SelectedAccentColor { get; set; }

    public virtual FontSize SelectedFontSize { get; set; }

    public virtual Link[] Themes { get; set; }

    public virtual Color[] AccentColors { get; set; }

    public virtual FontSize[] FontSizes { get; set; }

    public virtual IReactiveCommand Load { get; set; }

    public virtual IReactiveCommand Save { get; set; }

    public AppearanceViewModel()
    {
        _controller = IoC.Container.Resolve<AppearanceController>(new ViewModelDependency(this));
    }
}
