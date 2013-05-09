using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace CIS.UI.Features
{
    public class DialogService<TView, TViewModel>
        where TView : DialogBase
        where TViewModel : ViewModelBase
    {
        public virtual TView View { get; private set; }
        public virtual TViewModel ViewModel { get; set; }

        #region Methods

        public virtual Window CurrentWindow
        {
            get
            {
                return Application.Current.Windows
                    .OfType<Window>()
                    .Where(x => x.IsActive)
                    .SingleOrDefault();
            }
        }

        public virtual bool IsNotMainWindow(Window window)
        {
            return (!(window is MainView)); // && !(window is SplashScreenView));
        }

        public virtual TViewModel Show()
        {
            return Show(null, null);
        }

        public virtual TViewModel Show(object sender, string title, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(title))
                this.View.Title = title;

            if (args != null)
            {
                var viewModelInstance = args.OfType<TViewModel>().FirstOrDefault();
                if (viewModelInstance != null)
                    this.ViewModel.SerializeWith(viewModelInstance);
            }

            var result = View.ShowDialog();
            if (result != null && result == true)
                return this.ViewModel;
            else
                return null;
        }

        #endregion

        #region Constructors

        public DialogService()
        {
            this.View = Activator.CreateInstance<TView>();
            if (this.View.DataContext == null)
                this.View.DataContext = Activator.CreateInstance<TViewModel>();

            this.ViewModel = this.View.DataContext as TViewModel;

            // assign owner if View is not the main window
            if (this.IsNotMainWindow(View))
                View.Owner = this.CurrentWindow;
        }

        #endregion
    }
}
