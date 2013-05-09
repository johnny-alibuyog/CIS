﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Memberships
{
    public class User
    {
        private Guid _id;
        private int _version;
        private string _username;
        private string _password;
        private string _email;
        private Person _person;
        private ICollection<Role> _roles;

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

        public virtual string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public virtual string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public virtual string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public virtual Person Person
        {
            get { return _person; }
            set { _person = value; }
        }

        public virtual IEnumerable<Role> Roles
        {
            get { return _roles; }
            set { SyncRoles(value); }
        }

        #region Methods

        private void SyncRoles(IEnumerable<Role> items)
        {
            var itemsToInsert = items.Except(_roles).ToList();
            var itemsToRemove = _roles.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
                _roles.Add(item);

            // delete
            foreach (var item in itemsToRemove)
                _roles.Remove(item);
        }

        #endregion

        #region Constructors

        public User()
        {
            _roles = new Collection<Role>();
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as User;

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

        public static bool operator ==(User x, User y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(User x, User y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
