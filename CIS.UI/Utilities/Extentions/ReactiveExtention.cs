using CIS.Data.Common.Exception;
using CIS.UI.Features;
using ReactiveUI;
using System;
using System.Collections.Generic;

namespace CIS.UI.Utilities.Extentions;

public static class ReactiveExtention
{
    public static IReactiveList<T> ToReactiveList<T>(this IEnumerable<T> items)
    {
        return new ReactiveList<T>(items);
    }

    public static void Handle(this IObservable<Exception> observableException, IControllerBase handler, string message = null)
    {
        observableException.Subscribe(exception =>
        {
            var notification = exception switch
            {
                BusinessException businessException 
                    => businessException.Message,
                _ 
                    => !string.IsNullOrWhiteSpace(message) ? message : exception.Message
            };

            handler.MessageBox.Warn(notification);
        });
    }

    public static void Handle(this IObservable<Exception> observableException, ViewModelBase handler, string message = null)
    {
        observableException.Subscribe(exception =>
        {
            var notification = exception switch
            {
                BusinessException businessException
                    => businessException.Message,
                _
                    => !string.IsNullOrWhiteSpace(message) ? message : exception.Message
            };

            handler.MessageBox.Warn(notification);
        });
    }
}
