using CIS.Core.Domain.Security;
using CIS.Data;
using CIS.Data.Common.Exception;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.CommonDialogs;
using Common.Logging;
using NHibernate;
using ReactiveUI;
using System;
using System.Windows;

namespace CIS.UI.Features;

public abstract class ControllerBase<TViewModel>(TViewModel viewModel) : IControllerBase where TViewModel : ViewModelBase
{
    private static readonly ILog _log = LogManager.GetCurrentClassLogger();

    private readonly TViewModel _viewModel = viewModel;
    private readonly Lazy<IMessageBus> _messageBus = new(() => IoC.Container.Resolve<IMessageBus>());
    private readonly Lazy<IMessageBoxService> _messageBox = new(() => IoC.Container.Resolve<IMessageBoxService>());
    private readonly Lazy<ISessionProvider> _sessionProvider = new(() => IoC.Container.Resolve<ISessionProvider>());

    public virtual ILog Log => _log;

    public virtual TViewModel ViewModel => _viewModel;

    public virtual IMessageBus MessageBus => this._messageBus.Value;

    public virtual IMessageBoxService MessageBox => this._messageBox.Value;

    public virtual ISessionFactory SessionFactory => this.SessionProvider.SessionFactory;

    public virtual ISessionProvider SessionProvider => this._sessionProvider.Value;

    public virtual void DispatcherInvoke(Action action)
    {
        Application.Current.Dispatcher.Invoke(action);
    }

    public virtual void Authorize(params Role[] roles)
    {
        if (App.Context.User == null)
            throw new AuthenticationException("No user is currently logged-in.");

        if (App.Context.User.Has(roles) == false)
            throw new AuthorizationException("You are not authorized to perform this action.");
    }
}
