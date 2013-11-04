using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Firearms
{
    public class License
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Person _person;
        private Address _address;
        private Gun _gun;
        private string _licenseNumber;
        private string _controlNumber;
        private Nullable<DateTime> _issueDate;
        private Nullable<DateTime> _expiryDate;

        public virtual Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }

        public virtual Audit Audit
        {
            get { return _audit; }
            set { _audit = value; }
        }

        public virtual Person Person
        {
            get { return _person; }
            set { _person = value; }
        }

        public virtual Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public virtual Gun Gun
        {
            get { return _gun; }
            set { _gun = value; }
        }

        public virtual string LicenseNumber
        {
            get { return _licenseNumber; }
            set { _licenseNumber = value; }
        }

        public virtual string ControlNumber
        {
            get { return _controlNumber; }
            set { _controlNumber = value; }
        }

        public virtual Nullable<DateTime> IssueDate
        {
            get { return _issueDate; }
            set { _issueDate = value; }
        }

        public virtual Nullable<DateTime> ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }

        #region Methods

        public virtual void SerializeWith(License value)
        {
            this.LicenseNumber = value.LicenseNumber;
            this.ControlNumber = value.ControlNumber;
            this.IssueDate = value.IssueDate;
            this.ExpiryDate = value.ExpiryDate;
            this.Gun = value.Gun;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as License;

            if (that == null)
                return false;

            if (that.Id == Guid.Empty && this.Id == Guid.Empty)
                return object.ReferenceEquals(that, this);

            return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (this.Id != Guid.Empty)
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(License x, License y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(License x, License y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
