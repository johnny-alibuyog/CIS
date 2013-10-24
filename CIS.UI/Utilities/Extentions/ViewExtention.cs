using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.UI.Features;
using ReactiveUI;

namespace CIS.UI.Utilities.Extentions
{
    public static class ViewExtention
    {
        public static void InitializeViewModelAsync<T>(this IViewFor<T> view, Func<T> create) where T : ViewModelBase
        {
            //view.ViewModel = create.Invoke();
            Task.Factory.StartNew(() => Application.Current.Dispatcher.Invoke(() => view.ViewModel = create.Invoke()), TaskCreationOptions.AttachedToParent);
        }
    }
}
