using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.DependencyInjection;
using Common.Logging;
using NHibernate.Validator.Engine;
using ReactiveUI;

namespace CIS.UI.Features
{
    public abstract class ViewModelBase : ReactiveObject, IDataErrorInfo
    {
        private ValidatorEngine _validator;

        public virtual Nullable<bool> ActionResult { get; set; }

        public virtual object SerializeWith(object instance)
        {
            throw new NotImplementedException();
        }

        public virtual object SerializeInto(object instance)
        {
            throw new NotImplementedException();
        }

        protected virtual ValidatorEngine Validator
        {
            get
            {
                if (_validator == null)
                    _validator = IoC.Container.Resolve<ValidatorEngine>();

                return _validator;
            }
        }

        #region IDataErrorInfo Members

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

        public virtual bool IsValid { get; set; }

        public virtual string Error
        {
            get { return this[string.Empty]; }
        }

        public virtual string this[string columnName]
        {
            get
            {
                if (columnName == string.Empty)
                    return null;

                var invalidValues = this.Validator.Validate(this);
                this.IsValid = (invalidValues.Count() == 0);

                var invalidValue = invalidValues.FirstOrDefault(x => x.PropertyName == columnName);
                if (invalidValue != null)
                    return invalidValue.Message;
                else
                    return null;
            }
        }

        #endregion

        //public ViewModelBase()
        //{
        //    this.ValidationObservable.Subscribe(x => 
        //    {
        //        this.IsValid = this.IsObjectValid();
        //        raisePropertyChanged("IsValid");
        //    });
        //}
    }
}
