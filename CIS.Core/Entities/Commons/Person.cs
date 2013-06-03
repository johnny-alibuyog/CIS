using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using CIS.Core.Utilities.Extentions;

namespace CIS.Core.Entities.Commons
{
    public class Person
    {
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _suffix;
        private Nullable<Gender> _gender;
        private Nullable<DateTime> _birthDate;

        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = value.ToProperCase(); }
        }

        public virtual string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value.ToProperCase(); }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = value.ToProperCase(); }
        }

        public virtual string Suffix
        {
            get { return _suffix; }
            set { _suffix = value.ToProperCase(); }
        }

        public virtual Nullable<Gender> Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        public virtual Nullable<DateTime> BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; }
        }

        public virtual Nullable<int> Age
        {
            get { return this.BirthDate != null ? Person.ComputeAge(this.BirthDate.Value) : null; }
        }

        public virtual string Fullname
        {
            get { return Person.GetFullName(this); }
        }

        #region Static Members

        public static string GetFullName(Person person)
        {
            return
                (!string.IsNullOrWhiteSpace(person.FirstName) ? person.FirstName : string.Empty) +
                (!string.IsNullOrWhiteSpace(person.MiddleName) ? " " + person.MiddleName : string.Empty) +
                (!string.IsNullOrWhiteSpace(person.LastName) ? " " + person.LastName : string.Empty) +
                (!string.IsNullOrWhiteSpace(person.Suffix) ? " " + person.Suffix : string.Empty);
        }

        public static Nullable<int> ComputeAge(DateTime birthDate)
        {
            var now = DateTime.Now;
            var age = now.Year - birthDate.Year;

            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                age--;

            return age;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = !string.IsNullOrWhiteSpace(this.Fullname)
                    ? this.Fullname.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public override bool Equals(object obj)
        {
            var that = obj as Person;

            if (that == null)
                return false;

            if (string.IsNullOrWhiteSpace(that.Fullname) && string.IsNullOrWhiteSpace(this.Fullname))
                return object.ReferenceEquals(this, that);

            return that.Fullname.Equals(this.Fullname);
        }

        public static bool operator ==(Person x, Person y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Person x, Person y)
        {
            return !Equals(x, y);
        }

        #endregion

        #region Method Overrides

        public override string ToString()
        {
            return this.Fullname;
        }

        #endregion
    }
}
