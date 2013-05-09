using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Commons
{
    public class ContactType
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

        public ContactType() { }

        public ContactType(string id, string name)
        {
            _id = id;
            _name = name;
        }

        #endregion

        #region Static Members

        public static readonly ContactType Fax = new ContactType("FN", "Fax Number");
        public static readonly ContactType Mobile = new ContactType("MN", "Mobile Number");
        public static readonly ContactType Landline = new ContactType("LL", "Landline Number");
        public static readonly IEnumerable<ContactType> All = new ContactType[] 
        { 
            ContactType.Fax, 
            ContactType.Mobile, 
            ContactType.Landline, 
        };

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as ContactType;

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

        public static bool operator ==(ContactType x, ContactType y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(ContactType x, ContactType y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
