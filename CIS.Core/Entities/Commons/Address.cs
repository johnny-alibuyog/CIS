using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Commons
{
    public class Address
    {
        private string _address1;
        private string _address2;
        private string _barangay;
        private string _city;
        private string _province;

        public virtual string Address1
        {
            get { return _address1; }
            set { _address1 = value; }
        }

        public virtual string Address2
        {
            get { return _address2; }
            set { _address2 = value; }
        }

        public virtual string Barangay
        {
            get { return _barangay; }
            set { _barangay = value; }
        }

        public virtual string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public virtual string Province
        {
            get { return _province; }
            set { _province = value; }
        }

        #region Constructors

        public Address() { }

        public Address(string address1, string address2, string barangay, string city, string province)
        {
            _address1 = address1;
            _address2 = address2;
            _barangay = barangay;
            _city = city;
            _province = province;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                unchecked
                {
                    _hashCode = 17;
                    _hashCode = _hashCode * 23 + (!string.IsNullOrWhiteSpace(this.Address1) ? this.Address1.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (!string.IsNullOrWhiteSpace(this.Address2) ? this.Address2.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (!string.IsNullOrWhiteSpace(this.Barangay) ? this.Barangay.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (!string.IsNullOrWhiteSpace(this.City) ? this.City.GetHashCode() : 0);
                    _hashCode = _hashCode * 23 + (!string.IsNullOrWhiteSpace(this.Province) ? this.Province.GetHashCode() : 0);
                }
            }

            return _hashCode.Value;
        }

        public override bool Equals(object obj)
        {
            var that = obj as Address;

            if (that == null)
                return false;

            if (that.Address1 != this.Address1)
                return false;

            if (that.Address2 != this.Address2)
                return false;

            if (that.Barangay != this.Barangay)
                return false;

            if (that.City != this.City)
                return false;

            if (that.Province != this.Province)
                return false;

            return true;
        }

        public static bool operator ==(Address x, Address y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Address x, Address y)
        {
            return !Equals(x, y);
        }

        #endregion

        #region Method Overrides

        public override string ToString()
        {
            return
                (!string.IsNullOrWhiteSpace(this.Address1) ? this.Address1 : string.Empty) +
                (!string.IsNullOrWhiteSpace(this.Address2) ? " " + this.Address2 : string.Empty) +
                (!string.IsNullOrWhiteSpace(this.Barangay) ? ", " + this.Barangay : string.Empty) +
                (!string.IsNullOrWhiteSpace(this.City) ? ", " + this.City : string.Empty) +
                (!string.IsNullOrWhiteSpace(this.Province) ? ", " + this.Province : string.Empty);
        }

        #endregion
    }
}
