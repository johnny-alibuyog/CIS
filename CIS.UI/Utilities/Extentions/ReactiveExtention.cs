using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Data.Commons.Exceptions;
using CIS.UI.Features;
using ReactiveUI;

namespace CIS.UI.Utilities.Extentions
{
    public static class ReactiveExtention
    {
        public static IReactiveList<T> ToReactiveList<T>(this IEnumerable<T> items)
        {
            return new ReactiveList<T>(items);
        }

        public static void Handle(this IObservable<Exception> thrownExceptions, IControllerBase handler, string message = null)
        {
            thrownExceptions.Subscribe(x =>
            {
                var notification = string.Empty;

                if (x is BusinessException)
                    notification = x.Message;
                else if (!string.IsNullOrWhiteSpace(message))
                    notification = message;
                else
                    notification = x.Message;

                //var notification = x is BusinessException 
                //    ? x.Message : message != string.Empty 
                //    ? message : x.Message;

                handler.MessageBox.Warn(notification);
            });
        }
    }
}
