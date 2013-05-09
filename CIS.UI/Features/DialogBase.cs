using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;

namespace CIS.UI.Features
{
    public class DialogBase : ModernWindow
    {
        #region Static Members

        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached("DialogResult",
                typeof(bool?), typeof(DialogBase),
                new PropertyMetadata(DialogResultChanged)
            );

        private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
                window.DialogResult = e.NewValue as bool?;
        }

        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }

        #endregion

        #region Dependency Properties

        ///// <summary>
        ///// Identifies the Buttons dependency property.
        ///// </summary>
        //public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(IEnumerable<Button>), typeof(ModernDialog));

        ///// <summary>
        ///// Gets or sets the dialog buttons.
        ///// </summary>
        //public IEnumerable<Button> Buttons
        //{
        //    get { return (IEnumerable<Button>)GetValue(ButtonsProperty); }
        //    set { SetValue(ButtonsProperty, value); }
        //}

        #endregion

        public DialogBase()
        {
            this.Style = (Style)App.Current.Resources["EmptyWindow"];
            this.DefaultStyleKey = typeof(DialogBase);
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            // set the default owner to the app main window (if possible)
            if (Application.Current != null && Application.Current.MainWindow != this)
                this.Owner = Application.Current.MainWindow;
        }
    }
}
