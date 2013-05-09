using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CIS.Data;
using CIS.UI.Bootstraps.DependencyInjection;
using Common.Logging;
using NHibernate;
using NHibernate.Context;
using ReactiveUI;

namespace CIS.UI.Features
{
    public class ControllerBase<TViewModel> where TViewModel : ViewModelBase
    {
        private ILog _log;
        private TViewModel _viewModel;
        private IMessageBus _messageBus;
        private ISessionProvider _sessionProvider;

        internal virtual ILog Log
        {
            get
            {
                if (_log == null)
                    _log = IoC.Container.Resolve<ILog>();

                return _log;
            }
        }

        internal virtual TViewModel ViewModel
        {
            get { return _viewModel; }
        }

        internal virtual IMessageBus MessageBus
        {
            get
            {
                if (_messageBus == null)
                    _messageBus = IoC.Container.Resolve<IMessageBus>();

                return _messageBus;
            }
        }

        internal virtual ISessionFactory SessionFactory
        {
            get { return this.SessionProvider.SessionFactory; }
        }

        internal virtual ISessionProvider SessionProvider
        {
            get
            {
                if (_sessionProvider == null)
                {
                    _sessionProvider = IoC.Container.Resolve<ISessionProvider>();
                }

                return _sessionProvider;
            }
        }

        internal virtual void DispatcherInvoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        public ControllerBase(TViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
