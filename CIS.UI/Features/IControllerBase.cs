using System;
using CIS.Data;
using CIS.UI.Utilities.CommonDialogs;
using Common.Logging;
using NHibernate;
using ReactiveUI;

namespace CIS.UI.Features;

public interface IControllerBase
{
    void DispatcherInvoke(Action action);
    ILog Log { get; }
    IMessageBoxService MessageBox { get; }
    IMessageBus MessageBus { get; }
    ISessionFactory SessionFactory { get; }
    ISessionProvider SessionProvider { get; }
}
