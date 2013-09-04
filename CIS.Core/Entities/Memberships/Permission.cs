using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Memberships
{
    public class Permission
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

        public Permission() { }

        public Permission(string id, string name)
        {
            _id = id;
            _name = name;
        }

        #endregion

        #region Static Members

        public static readonly Permission Administer = new Permission("AD", "Administer");
        public static readonly Permission Approve = new Permission("AP", "Approve");
        public static readonly Permission Encode = new Permission("EC", "Encode");
        public static readonly IEnumerable<Permission> All = new Permission[] 
        { 
            Permission.Administer, 
            Permission.Approve, 
            Permission.Encode, 
        };

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Permission;

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

        public static bool operator ==(Permission x, Permission y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Permission x, Permission y)
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
