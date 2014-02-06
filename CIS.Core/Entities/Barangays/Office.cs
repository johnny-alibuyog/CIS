using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Barangays
{
    public class Office
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private string _name;
        private string _location;
        private Address _address;
        private Nullable<decimal> _clearanceFee;
        private ICollection<Incumbent> _incumbents;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
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

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public virtual Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public virtual Nullable<decimal> ClearanceFee
        {
            get { return _clearanceFee; }
            set { _clearanceFee = value; }
        }

        public virtual IEnumerable<Incumbent> Incumbents
        {
            get { return _incumbents; }
        }

        public Office()
        {
            _incumbents = new Collection<Incumbent>();
        }

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Office;

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

        public static bool operator ==(Office x, Office y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Office x, Office y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
