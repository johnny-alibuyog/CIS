using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.UI.Bootstraps.InversionOfControl;
using Microsoft.Win32;
using ReactiveUI;

namespace CIS.UI.Features
{
    public class DialogService<TView, TViewModel>
        where TView : DialogBase, IViewFor<TViewModel>
        where TViewModel : ViewModelBase
    {
        public virtual TView View
        {
            get;
            private set;
        }

        public virtual TViewModel ViewModel
        {
            get { return View.DataContext as TViewModel; }
            set { View.DataContext = value; }
        }

        #region Methods

        public virtual TViewModel ShowModal()
        {
            return ShowModal(null, null);
        }

        public virtual TViewModel ShowModal(object sender, string title, params object[] args)
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
            this.View = IoC.Container.Resolve<TView>();
            if (this.View.ViewModel == null)
                this.View.ViewModel = IoC.Container.Resolve<TViewModel>();

            // assign owner if View is not the main window
            if (!(this.View is MainView))
                View.Owner = App.CurrentWindow;
        }

        #endregion
    }
}
