using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.Configurations;
using CIS.UI.Utilities.Extentions;
using FirstFloor.ModernUI.Presentation;
using ReactiveUI;

namespace CIS.UI.Features.Settings.Appearances
{
    [HandleError]
    public class AppearanceController : ControllerBase<AppearanceViewModel>
    {
        private AppearanceConfiguration _originalAppearance;

        public AppearanceController(AppearanceViewModel viewModel) : base(viewModel)
        {
            this.Initiaize();

            this.ViewModel.ObservableForProperty(x => x.SelectedTheme)
                .Subscribe(x => AppearanceManager.Current.ThemeSource = x.Value.Source);

            this.ViewModel.ObservableForProperty(x => x.SelectedAccentColor)
                .Subscribe(x => AppearanceManager.Current.AccentColor = x.Value);

            this.ViewModel.ObservableForProperty(x => x.SelectedFontSize)
                .Subscribe(x => AppearanceManager.Current.FontSize = x.Value);

            this.ViewModel.Save = new ReactiveCommand();
            this.ViewModel.Save.Subscribe(x => Save());
            this.ViewModel.Save.ThrownExceptions.Handle(this);

            AppearanceManager.Current.WhenAny(
                x => x.ThemeSource,
                x => x.AccentColor,
                (themeSource, accentColor) => new
                {
                    ThemeSource = themeSource,
                    AccentColor = accentColor
                }
            )
            .Subscribe(x =>
            {
                // synchronizes the selected viewmodel theme with the actual theme used by the appearance manager.
                this.ViewModel.SelectedTheme = this.ViewModel.Themes.FirstOrDefault(o => o.Source.Equals(x.ThemeSource.Value));

                // and make sure accent color is up-to-date
                this.ViewModel.SelectedAccentColor = x.AccentColor.Value;
            });
        }

        private void Initiaize()
        {
            // add the default themes
            this.ViewModel.Themes = new Link[]
            {
                new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource },
                new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource }
            };

            // 9 accent colors from metro design principles
            /*_accentColors = new Color[]
             * {
                Color.FromRgb(0x33, 0x99, 0xff),   // blue
                Color.FromRgb(0x00, 0xab, 0xa9),   // teal
                Color.FromRgb(0x33, 0x99, 0x33),   // green
                Color.FromRgb(0x8c, 0xbf, 0x26),   // lime
                Color.FromRgb(0xf0, 0x96, 0x09),   // orange
                Color.FromRgb(0xff, 0x45, 0x00),   // orange red
                Color.FromRgb(0xe5, 0x14, 0x00),   // red
                Color.FromRgb(0xff, 0x00, 0x97),   // magenta
                Color.FromRgb(0xa2, 0x00, 0xff),   // purple            
            };*/

            // 20 accent colors from Windows Phone 8
            this.ViewModel.AccentColors = new Color[]
                {
                    Color.FromRgb(0xa4, 0xc4, 0x00),   // lime
                    Color.FromRgb(0x60, 0xa9, 0x17),   // green
                    Color.FromRgb(0x00, 0x8a, 0x00),   // emerald
                    Color.FromRgb(0x00, 0xab, 0xa9),   // teal
                    Color.FromRgb(0x1b, 0xa1, 0xe2),   // cyan
                    Color.FromRgb(0x00, 0x50, 0xef),   // cobalt
                    Color.FromRgb(0x6a, 0x00, 0xff),   // indigo
                    Color.FromRgb(0xaa, 0x00, 0xff),   // violet
                    Color.FromRgb(0xf4, 0x72, 0xd0),   // pink
                    Color.FromRgb(0xd8, 0x00, 0x73),   // magenta
                    Color.FromRgb(0xa2, 0x00, 0x25),   // crimson
                    Color.FromRgb(0xe5, 0x14, 0x00),   // red
                    Color.FromRgb(0xfa, 0x68, 0x00),   // orange
                    Color.FromRgb(0xf0, 0xa3, 0x0a),   // amber
                    Color.FromRgb(0xe3, 0xc8, 0x00),   // yellow
                    Color.FromRgb(0x82, 0x5a, 0x2c),   // brown
                    Color.FromRgb(0x6d, 0x87, 0x64),   // olive
                    Color.FromRgb(0x64, 0x76, 0x87),   // steel
                    Color.FromRgb(0x76, 0x60, 0x8a),   // mauve
                    Color.FromRgb(0x87, 0x79, 0x4e),   // taupe
                };

            this.ViewModel.FontSizes = Enum.GetValues(typeof(FontSize)).Cast<FontSize>().ToArray();

            this.ViewModel.SelectedTheme = App.Configuration.Apprearance.Theme == AppearanceThemeConfiguration.Empty
                ? this.ViewModel.Themes.FirstOrDefault(o => o.Source.Equals(AppearanceManager.Current.ThemeSource))
                : App.Configuration.Apprearance.Theme.Value;

            this.ViewModel.SelectedAccentColor = App.Configuration.Apprearance.Color == AppearanceColorConfiguration.Empty
                ? AppearanceManager.Current.AccentColor
                : App.Configuration.Apprearance.Color.Value;

            this.ViewModel.SelectedFontSize = App.Configuration.Apprearance.FontSize;

            _originalAppearance = new AppearanceConfiguration();
            _originalAppearance.FontSize = this.ViewModel.SelectedFontSize;
            _originalAppearance.Theme.Value = this.ViewModel.SelectedTheme;
            _originalAppearance.Color.Value = this.ViewModel.SelectedAccentColor;
        }

        private void Save()
        {
            var confirmed = this.MessageBox.Confirm("Do you want to save changes?", "Confirmation", true);
            if (confirmed == null)
                return;

            if (confirmed == true)
            {
                App.Configuration.Apprearance.Theme.Value = this.ViewModel.SelectedTheme;
                App.Configuration.Apprearance.Color.Value = this.ViewModel.SelectedAccentColor;
                App.Configuration.Apprearance.FontSize = this.ViewModel.SelectedFontSize;
                App.Configuration.Provider.Write(App.Configuration);
                App.Configuration.Apprearance.Apply();

                _originalAppearance = new AppearanceConfiguration();
                _originalAppearance.FontSize = this.ViewModel.SelectedFontSize;
                _originalAppearance.Theme.Value = this.ViewModel.SelectedTheme;
                _originalAppearance.Color.Value = this.ViewModel.SelectedAccentColor;
            }
            else
            {
                this.ViewModel.SelectedFontSize = _originalAppearance.FontSize;
                this.ViewModel.SelectedTheme = _originalAppearance.Theme.Value;
                this.ViewModel.SelectedAccentColor = _originalAppearance.Color.Value;
            }
        }
    }
}
