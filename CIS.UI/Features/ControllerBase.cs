using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CIS.Core.Entities.Memberships;
using CIS.Data;
using CIS.Data.Commons.Exceptions;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.CommonDialogs;
using Common.Logging;
using NHibernate;
using NHibernate.Context;
using ReactiveUI;

namespace CIS.UI.Features
{
    public abstract class ControllerBase<TViewModel> : IControllerBase where TViewModel : ViewModelBase
    {
        private static ILog _log;
        private TViewModel _viewModel;
        private IMessageBus _messageBus;
        private IMessageBoxService _messageBox;
        private ISessionProvider _sessionProvider;

        public virtual ILog Log
        {
            get
            {
                if (_log == null)
                    _log = Common.Logging.LogManager.GetCurrentClassLogger(); //IoC.Container.Resolve<ILog>();

                return _log;
            }
        }

        public virtual TViewModel ViewModel
        {
            get { return _viewModel; }
        }

        public virtual IMessageBus MessageBus
        {
            get
            {
                if (_messageBus == null)
                    _messageBus = IoC.Container.Resolve<IMessageBus>();

                return _messageBus;
            }
        }

        public virtual IMessageBoxService MessageBox
        {
            get
            {
                if (_messageBox == null)
                    _messageBox = IoC.Container.Resolve<IMessageBoxService>();

                return _messageBox;
            }
        }

        public virtual ISessionFactory SessionFactory
        {
            get { return this.SessionProvider.SessionFactory; }
        }

        public virtual ISessionProvider SessionProvider
        {
            get
            {
                if (_sessionProvider == null)
                    _sessionProvider = IoC.Container.Resolve<ISessionProvider>();

                return _sessionProvider;
            }
        }

        public virtual void DispatcherInvoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        //public virtual bool Authorize(params Role[] roles)
        //{
        //    if (App.Data.User == null)
        //    {
        //        _messageBox.Warn("No user is currently logged-in.");
        //        return false;
        //    }

        //    if (App.Data.User.Has(roles) == false)
        //    {
        //        _messageBox.Warn(string.Format("User is not authorized for this action."));
        //        return false;
        //    }

        //    return true;
        //}

        public virtual void Authorize(params Role[] roles)
        {
            if (App.Data.User == null)
                throw new BusinessException("No user is currently logged-in.");

            if (App.Data.User.Has(roles) == false)
                throw new BusinessException("You are not authorized for this action.");
        }

        public ControllerBase(TViewModel viewModel)
        {
            _viewModel = viewModel;

        }
    }
}
