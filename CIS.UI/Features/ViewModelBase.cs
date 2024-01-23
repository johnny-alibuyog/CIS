using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CIS.UI.Bootstraps.InversionOfControl;
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

    public virtual bool IsValid { get; private set; }

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

            var message = invalidValues
                .Where(x => x.PropertyName == columnName)
                .Select(x => x.Message)
                .FirstOrDefault()
;
            return message;
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

    #region Equality Comparer

    public static bool operator ==(ViewModelBase one, ViewModelBase two)
    {
        return EqualOperator(one, two);
    }

    protected static bool EqualOperator(ViewModelBase left, ViewModelBase right)
    {
        if (left is null ^ right is null)
            return false;

        return ReferenceEquals(left, right) || left.Equals(right);
    }

    public static bool operator !=(ViewModelBase one, ViewModelBase two)
    {
        return NotEqualOperator(one, two);
    }

    protected static bool NotEqualOperator(ViewModelBase left, ViewModelBase right)
    {
        return !EqualOperator(left, right);
    }

    protected virtual IEnumerable<object> GetEqualityValues()
    {
        yield return this;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var that = (ViewModelBase)obj;

        return this.GetEqualityValues().SequenceEqual(that.GetEqualityValues());
    }

    public override int GetHashCode()
    {
        return GetEqualityValues()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    #endregion
}
