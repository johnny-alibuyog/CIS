using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Lookups
{
    public class Purpose
    {
        private string _id;
        private string _name;

        public virtual string Id 
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string Name 
        {
            get { return _name; }
            protected set { _name = value; }
        }

        #region Constructors

        public Purpose() { }

        public Purpose(string id, string name)
        {
            _id = id;
            _name = name;
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Purpose;

            if (that == null)
                return false;

            if (string.IsNullOrWhiteSpace(that.Id) && string.IsNullOrWhiteSpace(this.Id))
                return object.ReferenceEquals(that, this);

            return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (!string.IsNullOrWhiteSpace(this.Id))
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(Purpose x, Purpose y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Purpose x, Purpose y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
