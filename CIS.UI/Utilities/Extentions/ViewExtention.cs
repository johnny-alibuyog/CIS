using System;
using CIS.UI.Features;
using ReactiveUI;

namespace CIS.UI.Utilities.Extentions;

public static class ViewExtention
{
    public static void InitializeViewModel<T>(this IViewFor<T> view, Func<T> create) where T : ViewModelBase
    {
        view.ViewModel = create.Invoke();
        //Task.Factory.StartNew(() => Application.Current.Dispatcher.Invoke(() => view.ViewModel = create.Invoke()), TaskCreationOptions.AttachedToParent);
    }
}
