using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Mayors
{
    public abstract class Official
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Person _person;

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

        public virtual Person Person
        {
            get { return _person; }
            set { _person = value; }
        }

        #region Methods

        public virtual void SerializeWith(Official value)
        {
            this.Person = value.Person;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Official;

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

        public static bool operator ==(Official x, Official y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Official x, Official y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
