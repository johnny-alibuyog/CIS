using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using FirstFloor.ModernUI.Presentation;

namespace CIS.UI.Utilities.Configurations
{
    [Serializable()]
    public class AppearanceConfiguration
    {
        private FontSize _fontSize;
        private AppearanceColorConfiguration _color;
        private AppearanceThemeConfiguration _theme;

        public virtual FontSize FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        public virtual AppearanceColorConfiguration Color 
        {
            get { return _color; }
            set { _color = value; } 
        }

        public virtual AppearanceThemeConfiguration Theme 
        {
            get { return _theme; }
            set { _theme = value; } 
        }

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
}
