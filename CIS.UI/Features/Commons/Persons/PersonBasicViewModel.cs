﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;
using NHibernate.Validator.Constraints;

namespace CIS.UI.Features.Commons.Persons
{
    public class PersonBasicViewModel : ViewModelBase
    {
        [NotNullNotEmpty(Message = "First name is mandatory.")]
        public virtual string FirstName { get; set; }

        public virtual string MiddleName { get; set; }

        [NotNullNotEmpty(Message = "Last name is mandatory.")]
        public virtual string LastName { get; set; }

        public virtual string Suffix { get; set; }

        public virtual string FullName
        {
            get 
            { 
                return string.Format("{0} {1} {2} {3}", 
                    this.FirstName ?? string.Empty, 
                    this.MiddleName ?? string.Empty, 
                    this.LastName ?? string.Empty,
                    this.Suffix ?? string.Empty
                )
                .ToProperCase(); 
            }
        }

        public PersonBasicViewModel() { }

        public PersonBasicViewModel(PersonBasic value)
        {
            this.SerializeWith(value);
        }

        public override string ToString()
        {
            return this.FullName;
        }

        public override object SerializeWith(object instance)
        {
            if (instance == null)
                return null;

            if (instance == PersonBasicViewModel.Empty)
                return null;

            if (instance is PersonBasicViewModel)
            {
                var source = instance as PersonBasicViewModel;
                var target = this;

                target.FirstName = source.FirstName;
                target.MiddleName = source.MiddleName;
                target.LastName = source.LastName;
                target.Suffix = source.Suffix;
                return target;
            }
            else if (instance is PersonBasic)
            {
                var source = instance as PersonBasic;
                var target = this;

                target.FirstName = source.FirstName;
                target.MiddleName = source.MiddleName;
                target.LastName = source.LastName;
                target.Suffix = source.Suffix;
                return target;
            }

            return null;
        }

        public override object DeserializeInto(object instance)
        {
            if (instance == null)
                return null;

            if (this == PersonBasicViewModel.Empty)
                return null;

            if (instance is PersonBasicViewModel)
            {
                var source = this;
                var target = instance as PersonBasicViewModel;

                target.SerializeWith(source);
                return target;
            }
            else if (instance is PersonBasic)
            {
                var source = this;
                var target = instance as PersonBasic;

                target.FirstName = source.FirstName;
                target.MiddleName = source.MiddleName;
                target.LastName = source.LastName;
                target.Suffix = source.Suffix;

                return target;
            }

            return null;
        }

        public static readonly PersonBasicViewModel Empty = new PersonBasicViewModel();

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as PersonBasicViewModel;

            if (that == null)
                return false;

            if (that.FirstName != this.FirstName)
                return false;

            if (that.MiddleName != this.MiddleName)
                return false;

            if (this.LastName != this.LastName)
                return false;

            if (this.Suffix != this.Suffix)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                unchecked
                {
                    _hashCode = 17;
                    _hashCode = _hashCode * 23 + (!string.IsNullOrWhiteSpace(this.FirstName) ? this.FirstName.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (!string.IsNullOrWhiteSpace(this.MiddleName) ? this.MiddleName.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (!string.IsNullOrWhiteSpace(this.LastName) ? this.LastName.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (!string.IsNullOrWhiteSpace(this.Suffix) ? this.Suffix.GetHashCode() : 0);
                }
            }

            return _hashCode.Value;
        }

        public static bool operator ==(PersonBasicViewModel x, PersonBasicViewModel y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(PersonBasicViewModel x, PersonBasicViewModel y)
        {
            return !Equals(x, y);
        }

        #endregion

    }
}