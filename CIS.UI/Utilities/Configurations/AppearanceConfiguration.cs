using System;
using FirstFloor.ModernUI.Presentation;

namespace CIS.UI.Utilities.Configurations;

[Serializable()]
public class AppearanceConfiguration
{
    public virtual FontSize FontSize { get; set; }
    public virtual AppearanceColorConfiguration Color { get; set; }
    public virtual AppearanceThemeConfiguration Theme { get; set; }

    public AppearanceConfiguration()
    {
        this.Color = new AppearanceColorConfiguration();
        this.Theme = new AppearanceThemeConfiguration();
    }

    public virtual void Apply()
    {
        AppearanceManager.Current.FontSize = this.FontSize;

        if (this.Color != AppearanceColorConfiguration.Empty)
            AppearanceManager.Current.AccentColor = this.Color.Value;

        if (this.Theme != AppearanceThemeConfiguration.Empty)
            AppearanceManager.Current.ThemeSource = this.Theme.Value.Source;
    }
}
