using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;

namespace CIS.Core.Entities.Polices
{
    public class Station
    {
        private Guid _id;
        private int _version;
        private Audit _audit;
        private Picture _logo;
        private string _name;
        private string _location;
        private string _clearanceValidity;
        private Address _address;
        private ICollection<Officer> _officers;

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

        public virtual Picture Logo
        {
            get { return _logo; }
            set { _logo = value; }
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

        public virtual string ClearanceValidity
        {
            get { return _clearanceValidity; }
            set { _clearanceValidity = value; }
        }

        public virtual Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public virtual IEnumerable<Officer> Officers
        {
            get { return _officers; }
            set { SyncOfficers(value); }
        }

        #region Methods

        private void SyncOfficers(IEnumerable<Officer> items)
        {
            foreach (var item in items)
                item.Station = this;

            var itemsToInsert = items.Except(_officers).ToList();
            var itemsToUpdate = _officers.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _officers.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                item.Station = this;
                _officers.Add(item);
            }

            // update
            foreach (var item in itemsToUpdate)
            {
                var value = items.Single(x => x == item);
                item.SerializeWith(value);
            }

            // delete
            foreach (var item in itemsToRemove)
            {
                item.Station = null;
                _officers.Remove(item);
            }
        }

        public virtual void AddOfficer(Officer item)
        {
            item.Station = this;
            _officers.Add(item);
        }

        #endregion

        #region Constructors

        public Station()
        {
            _logo = new Picture();
            _officers = new Collection<Officer>();
        }

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as Station;

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

        public static bool operator ==(Station x, Station y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Station x, Station y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
