using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using CIS.Core.Domain.Security;
using CIS.Data;
using CIS.Data.Common.Exception;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.CommonDialogs;
using NHibernate;
using NHibernate.Validator.Engine;
using ReactiveUI;

namespace CIS.UI.Features;

public abstract class ViewModelBase : ReactiveObject, IDataErrorInfo
{
    private ValidatorEngine _validator;

    public virtual bool? ActionResult { get; set; }

    public virtual object SerializeWith(object instance)
    {
        throw new NotImplementedException();
    }

    public virtual object DeserializeInto(object instance)
    {
        throw new NotImplementedException();
    }

    protected virtual ValidatorEngine Validator => _validator ??= IoC.Container.Resolve<ValidatorEngine>();

    #region Close Dialog Overload

    public virtual void Close()
    {
        this.Close(true);
    }

    public virtual void Close(bool result)
    {
        this.ActionResult = result;
    }

    #endregion

    #region IDataErrorInfo Members

    public virtual bool IsValid { get; private set; } = true;

    public virtual string Error
    {
        get
        {
            var invalidValues = this.Validator.Validate(this);

            this.IsValid = !invalidValues.Any();

            return string.Join(Environment.NewLine, invalidValues.Select(x => x.Message));
        }
    }

    public virtual string this[string columnName]
    {
        get
        {
            if (columnName == string.Empty)
                return null;

            var invalidValues = this.Validator.Validate(this);

            this.IsValid = !invalidValues.Any();

            var messages = invalidValues
                .Where(x => x.PropertyName == columnName)
                .Select(x => x.Message)
                .ToList();
            
            return string.Join(Environment.NewLine, messages);
        }
    }

    public virtual void Revalidate()
    {
        var invalidValues = this.Validator.Validate(this);
        this.IsValid = (!invalidValues.Any());
    }

    public virtual IObservable<bool> IsValidObservable()
    {
        return this.WhenAnyValue(x => x.IsValid);
    }

    #endregion

    #region Constructors

    public ViewModelBase()
    {
        //var type = this.GetType();

        //var children = type.GetProperties()
        //    .Where(x =>
        //        x.PropertyType == typeof(ViewModelBase) &&
        //        x.GetCustomAttributes(false).OfType<ValidAttribute>().Any()   //(typeof(ValidAttribute), true).Any()
        //    )
        //    .Select(x => x.GetValue(this) as ViewModelBase)
        //    .ToList();

        //Console.WriteLine(children.Count);

        //children.WhenAnyObservable(x => x.

        //foreach (var child in children)
        //{
        //    child..WhenAny(x => x.IsValid, x => x.Value)().Subscribe(x =>  x
        //}
    }

    #endregion

    #region IDataErrorInfo Members Old Implementation

    //public virtual string Error
    //{
    //    get { return this[string.Empty]; }
    //}

    //public virtual string this[string columnName]
    //{
    //    get
    //    {
    //        if (columnName == string.Empty)
    //            return null;

    //        var invalidValue = this.Validator
    //            .ValidatePropertyValue(this, columnName)
    //            .FirstOrDefault();

    //        if (invalidValue != null)
    //            return invalidValue.Message;
    //        else
    //            return null;
    //    }
    //}

    //public virtual bool IsValid
    //{
    //    get
    //    {
    //        var invalidValues = this.Validator.Validate(this);
    //        if (invalidValues == null || invalidValues.Count() == 0)
    //            return true;
    //        else
    //            return false;
    //    }
    //}

    #endregion

    #region Services

    private readonly Lazy<IMessageBus> _messageBus = new(() => IoC.Container.Resolve<IMessageBus>());
    private readonly Lazy<IMessageBoxService> _messageBox = new(() => IoC.Container.Resolve<IMessageBoxService>());
    private readonly Lazy<ISessionProvider> _sessionProvider = new(() => IoC.Container.Resolve<ISessionProvider>());

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
    #endregion
}
