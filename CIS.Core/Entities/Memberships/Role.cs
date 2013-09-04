using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Memberships
{
    public class Role
    {
        private string _id;
        private string _name;

        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #region Constructors

        public Role() { }

        public Role(string id, string name)
        {
            _id = id;
            _name = name;
        }

        #endregion

        #region Static Members

        public static readonly Role Administrator = new Role("A", "Administrator");
        public static readonly Role Police = new Role("P", "Police");
        public static readonly Role Barangay = new Role("B", "Barangay");
        public static readonly Role Mayors = new Role("M", "Mayors");

        public static readonly IEnumerable<Role> All = new Role[] 
        { 
            Administrator, 
            Police, 
            Barangay, 
            Mayors 
        };

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Role;

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

        public static bool operator ==(Role x, Role y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Role x, Role y)
        {
            return !Equals(x, y);
        }

        #endregion

        #region Method Overrides

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
